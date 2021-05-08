#nullable enable

using System;
using System.Security.Cryptography.X509Certificates;

namespace RemoteDesktopServicesCertificateSelector.Data {

    public class Certificate {

        public string? name { get; }
        public string? issuerName { get; }
        public DateTime expirationDate { get; }
        public string thumbprint { get; }
        public bool isDefaultSelfSigned { get; }
        internal X509Certificate2? windowsCertificate { get; }

        public Certificate(string thumbprint) {
            this.thumbprint = thumbprint;
        }

        private Certificate(string thumbprint, string name, string issuerName, DateTime expirationDate, bool isDefaultSelfSigned): this(thumbprint) {
            this.name                = name;
            this.issuerName          = issuerName;
            this.expirationDate      = expirationDate;
            this.isDefaultSelfSigned = isDefaultSelfSigned;
        }

        public Certificate(X509Certificate2 cert, bool isDefaultSelfSigned): this(cert.Thumbprint!,
            string.IsNullOrWhiteSpace(cert.FriendlyName) ? cert.GetNameInfo(X509NameType.SimpleName, false)! : cert.FriendlyName,
            cert.GetNameInfo(X509NameType.SimpleName, true)!, cert.NotAfter, isDefaultSelfSigned) {
            windowsCertificate = cert;
        }

        protected bool Equals(Certificate other) {
            return thumbprint == other.thumbprint;
        }

        public override bool Equals(object? obj) {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj.GetType() == GetType() && Equals((Certificate) obj);
        }

        public static bool operator ==(Certificate? left, Certificate? right) {
            return Equals(left, right);
        }

        public static bool operator !=(Certificate? left, Certificate? right) {
            return !Equals(left, right);
        }

        public override int GetHashCode() {
            return thumbprint.GetHashCode();
        }

    }

}