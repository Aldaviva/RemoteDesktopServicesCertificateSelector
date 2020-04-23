#nullable enable

using System;

namespace CertificateUpdater.Data {

    public class Certificate {

        public string? name { get; }
        public string? issuerName { get; }
        public DateTime expirationDate { get; }
        public bool isExpired => expirationDate <= DateTime.Now;
        public string thumbprint { get; }
        public bool isDefaultSelfSigned { get; }

        public Certificate(string thumbprint) {
            this.thumbprint = thumbprint;
        }

        public Certificate(string thumbprint, string name, string issuerName, DateTime expirationDate, bool isDefaultSelfSigned): this(thumbprint) {
            this.name = name;
            this.issuerName = issuerName;
            this.expirationDate = expirationDate;
            this.isDefaultSelfSigned = isDefaultSelfSigned;
        }

        private bool equals(Certificate other) {
            return thumbprint == other.thumbprint;
        }

        public override bool Equals(object obj) {
            if (obj is null) {
                return false;
            } else if (ReferenceEquals(this, obj)) {
                return true;
            }

            return obj.GetType() == GetType() && equals((Certificate) obj);
        }

        public override int GetHashCode() {
            return thumbprint.GetHashCode();
        }

    }

}