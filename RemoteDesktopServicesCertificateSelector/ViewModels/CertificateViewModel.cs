using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Prism.Commands;
using RemoteDesktopServicesCertificateSelector.Data;
using RemoteDesktopServicesCertificateSelector.Managers;

#nullable enable

namespace RemoteDesktopServicesCertificateSelector.ViewModels {

    public class CertificateViewModel: INotifyPropertyChanged {

        private readonly CertificateManager certificateManager;

        public Certificate certificate { get; }

        public string? name => certificate.name;
        public string? issuerName => certificate.issuerName;
        public DateTime expirationDate => certificate.expirationDate;
        public string thumbprint => certificate.thumbprint;
        public bool isExpired => expirationDate <= DateTime.Now;

        private bool _isActive;

        public bool isActive {
            get => _isActive;
            set {
                if (_isActive == value) return;
                _isActive = value;
                onPropertyChanged();
            }
        }

        public DelegateCommand copyThumbprintCommand { get; }

        public DelegateCommand openCertificateCommand { get; }

        public CertificateViewModel(Certificate certificate, CertificateManager certificateManager) {
            this.certificateManager = certificateManager;
            this.certificate        = certificate;
            copyThumbprintCommand   = new DelegateCommand(copyThumbprint);
            openCertificateCommand  = new DelegateCommand(openCertificate);
        }

        private void openCertificate() {
            certificateManager.openCertificate(certificate);
        }

        private void copyThumbprint() {
            Clipboard.SetText(thumbprint);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void onPropertyChanged([CallerMemberName] string? propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}