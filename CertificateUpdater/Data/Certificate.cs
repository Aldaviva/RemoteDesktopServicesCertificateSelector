#nullable enable

using System;
using System.Windows;
using Prism.Commands;

namespace CertificateUpdater.Data {

    public class Certificate {

        public string? name { get; }
        public string? issuerName { get; }
        public DateTime expirationDate { get; }
        public bool isExpired => expirationDate <= DateTime.Now;
        public string thumbprint { get; }
        public bool isDefaultSelfSigned { get; }
        public DelegateCommand copyThumbprintCommand { get; }

        public Certificate(string thumbprint) {
            this.thumbprint       = thumbprint;
            copyThumbprintCommand = new DelegateCommand(copyThumbprint);
        }

        public Certificate(string thumbprint, string name, string issuerName, DateTime expirationDate, bool isDefaultSelfSigned): this(thumbprint) {
            this.name                = name;
            this.issuerName          = issuerName;
            this.expirationDate      = expirationDate;
            this.isDefaultSelfSigned = isDefaultSelfSigned;
        }

        private void copyThumbprint() {
            Clipboard.SetText(thumbprint);
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