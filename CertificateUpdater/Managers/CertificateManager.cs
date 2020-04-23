#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using CertificateUpdater.Data;
using CertificateUpdater.Resources;
using Microsoft.Management.Infrastructure;

namespace CertificateUpdater.Managers {

    public interface CertificateManager {

        IEnumerable<Certificate> installedCertificates { get; }

        Certificate activeTerminalServicesCertificate { get; set; }

        Task launchCertificateManagementConsole();

    }

    public class CertificateManagerImpl: CertificateManager, IDisposable {

        private const string TERMINALSERVICES_NAMESPACE = @"root/cimv2/terminalservices";
        private const string SSL_CERTIFICATE_SHA1_HASH = "SSLCertificateSHA1Hash";
        private const string REMOTE_DESKTOP_STORE = "Remote Desktop";
        private const string USE_DEFAULT_SELF_SIGNED_CERT = "0000000000000000000000000000000000000000";

        private readonly CimSession cimSession = CimSession.Create(null);

        public void Dispose() {
            cimSession?.Close();
            cimSession?.Dispose();
        }

        public IEnumerable<Certificate> installedCertificates => new[] {
            new X509Store(StoreName.My, StoreLocation.LocalMachine),
            new X509Store(REMOTE_DESKTOP_STORE, StoreLocation.LocalMachine)
        }.SelectMany(store => {
            store.Open(OpenFlags.ReadOnly);
            IEnumerable<Certificate> certificates = store.Certificates
                .Find(X509FindType.FindByApplicationPolicy,
                    Oid.FromFriendlyName("Server Authentication", OidGroup.EnhancedKeyUsage).Value, false)
                .Cast<X509Certificate2>()
                .Select(cert => new Certificate(cert.Thumbprint, string.IsNullOrWhiteSpace(cert.FriendlyName)
                        ? cert.GetNameInfo(X509NameType.SimpleName, false)
                        : cert.FriendlyName,
                    cert.GetNameInfo(X509NameType.SimpleName, true), cert.NotAfter, store.Name == REMOTE_DESKTOP_STORE));
            store.Close();
            return certificates;
        });

        private CimInstance getRdpTcpGeneralSetting() {
            return cimSession.QueryInstances(TERMINALSERVICES_NAMESPACE, "WQL",
                $"select {SSL_CERTIFICATE_SHA1_HASH} from Win32_TSGeneralSetting where TerminalName='RDP-tcp'").First();
        }

        public Certificate activeTerminalServicesCertificate {
            get {
                CimInstance rdpTcpGeneralSetting = getRdpTcpGeneralSetting();
                string activeThumbprint = (string) rdpTcpGeneralSetting.CimInstanceProperties[SSL_CERTIFICATE_SHA1_HASH].Value;
                return installedCertificates.FirstOrDefault(installedCert => installedCert.thumbprint == activeThumbprint) ??
                       new Certificate(activeThumbprint);
            }
            set {
                CimInstance rdpTcpGeneralSetting = getRdpTcpGeneralSetting();
                rdpTcpGeneralSetting.CimInstanceProperties[SSL_CERTIFICATE_SHA1_HASH].Value =
                    value.isDefaultSelfSigned ? USE_DEFAULT_SELF_SIGNED_CERT : value.thumbprint;
                cimSession.ModifyInstance(rdpTcpGeneralSetting);
            }
        }

        public async Task launchCertificateManagementConsole() {
            string mscFilename = Path.GetTempFileName();
            File.WriteAllBytes(mscFilename, Resource1.CertificatesMsc);

            var startInfo = new ProcessStartInfo("mmc.exe", mscFilename);
            Process.Start(startInfo);

            await Task.Delay(5000);
            File.Delete(mscFilename);
        }

    }

}