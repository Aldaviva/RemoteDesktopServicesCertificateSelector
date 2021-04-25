#nullable enable

using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.Management.Infrastructure;
using Prism.Commands;
using Prism.Mvvm;
using RemoteDesktopServicesCertificateSelector.Data;
using RemoteDesktopServicesCertificateSelector.Managers;

namespace RemoteDesktopServicesCertificateSelector.ViewModels {

    public class MainWindowViewModel: BindableBase {

        private readonly CertificateManager certificateManager;

        public CertificateViewModel? activeCertificateViewModel => installedCertificates.FirstOrDefault(certificate => certificate.isActive);

        public DelegateCommand manageCertificatesCommand { get; }
        public DelegateCommand saveCommand { get; }

        public string title { get; } = "Remote Desktop Services Certificate";

        public ObservableCollection<CertificateViewModel> installedCertificates { get; } = new();

        public MainWindowViewModel(CertificateManager certificateManager) {
            this.certificateManager = certificateManager;

            Certificate activeCertificate = certificateManager.activeTerminalServicesCertificate;
            installedCertificates.AddRange(certificateManager.installedCertificates
                .Select(certificate => new CertificateViewModel(certificate) { isActive = certificate == activeCertificate }));

            manageCertificatesCommand = new DelegateCommand(manageCertificates);
            saveCommand               = new DelegateCommand(save);
        }

        public void setActive(CertificateViewModel selectedItem) {
            if (activeCertificateViewModel is not null) {
                activeCertificateViewModel.isActive = false;
            }

            selectedItem.isActive = true;
        }

        private void manageCertificates() {
            certificateManager.launchCertificateManagementConsole();
        }

        private void save() {
            if (activeCertificateViewModel?.certificate is { } selectedCertificate) {
                try {
                    certificateManager.activeTerminalServicesCertificate = selectedCertificate;
                    MessageBox.Show($"Remote Desktop Connection is now using the \"{selectedCertificate.name}\" certificate.",
                        "Connection updated", MessageBoxButton.OK, MessageBoxImage.Information);
                } catch (CimException e) {
                    MessageBox.Show(e.Message, "Failed to change certificate", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            } else {
                MessageBox.Show("No certificate selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }

}