#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Management.Infrastructure;
using RemoteDesktopServicesCertificateSelector.Data;
using RemoteDesktopServicesCertificateSelector.Properties;

namespace RemoteDesktopServicesCertificateSelector.Managers; 

public interface CertificateManager {

    IEnumerable<Certificate> installedCertificates { get; }

    Certificate activeTerminalServicesCertificate { get; set; }

    Task launchCertificateManagementConsole();

    void openCertificate(Certificate certificate);

}

public class CertificateManagerImpl: CertificateManager, IDisposable {

    private const string TERMINALSERVICES_NAMESPACE   = @"root/cimv2/terminalservices";
    private const string SSL_CERTIFICATE_SHA1_HASH    = "SSLCertificateSHA1Hash";
    private const string REMOTE_DESKTOP_STORE         = "Remote Desktop";
    private const string USE_DEFAULT_SELF_SIGNED_CERT = "0000000000000000000000000000000000000000";

    private readonly CimSession cimSession = CimSession.Create(null);

    private string? mostRecentMscFilename;

    public void Dispose() {
        if (mostRecentMscFilename is not null) {
            File.Delete(mostRecentMscFilename);
            mostRecentMscFilename = null;
        }

        cimSession.Close();
        cimSession.Dispose();
    }

    public IEnumerable<Certificate> installedCertificates => new[] {
        new X509Store(StoreName.My, StoreLocation.LocalMachine),
        new X509Store(REMOTE_DESKTOP_STORE, StoreLocation.LocalMachine)
    }.SelectMany(store => {
        store.Open(OpenFlags.ReadOnly);
        IEnumerable<Certificate> certificates = store.Certificates
            .Find(X509FindType.FindByApplicationPolicy, Oid.FromFriendlyName("Server Authentication", OidGroup.EnhancedKeyUsage).Value, false)
            .Cast<X509Certificate2>()
            .Select(cert => new Certificate(cert, store.Name == REMOTE_DESKTOP_STORE));
        store.Close();
        return certificates;
    });

    private CimInstance getRdpTcpGeneralSetting() {
        return cimSession.QueryInstances(TERMINALSERVICES_NAMESPACE, "WQL", $"select {SSL_CERTIFICATE_SHA1_HASH} from Win32_TSGeneralSetting where TerminalName='RDP-tcp'").First();
    }

    public Certificate activeTerminalServicesCertificate {
        get {
            string activeThumbprint = (string) getRdpTcpGeneralSetting().CimInstanceProperties[SSL_CERTIFICATE_SHA1_HASH].Value;
            return installedCertificates.FirstOrDefault(installedCert => installedCert.thumbprint == activeThumbprint) ?? new Certificate(activeThumbprint);
        }
        set {
            CimInstance rdpTcpGeneralSetting = getRdpTcpGeneralSetting();
            rdpTcpGeneralSetting.CimInstanceProperties[SSL_CERTIFICATE_SHA1_HASH].Value = value.isDefaultSelfSigned ? USE_DEFAULT_SELF_SIGNED_CERT : value.thumbprint;
            cimSession.ModifyInstance(rdpTcpGeneralSetting);
        }
    }

    public async Task launchCertificateManagementConsole() {
        if (mostRecentMscFilename != null) {
            File.Delete(mostRecentMscFilename);
        }

        mostRecentMscFilename = Path.GetTempFileName();
        File.WriteAllBytes(mostRecentMscFilename, Resources.CertificatesMsc);

        Process.Start("mmc.exe", mostRecentMscFilename);

        await Task.Delay(5000);
        File.Delete(mostRecentMscFilename);
        mostRecentMscFilename = null;
    }

    public void openCertificate(Certificate certificate) {
        if (certificate.windowsCertificate is not null) {
            X509Certificate2UI.DisplayCertificate(certificate.windowsCertificate);
        }
    }

}