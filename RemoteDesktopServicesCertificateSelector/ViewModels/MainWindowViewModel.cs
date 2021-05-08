#nullable enable

using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public DelegateCommand refreshCommand { get; }

        public string title { get; } = "Remote Desktop Services Certificate Selector";

        public ObservableCollection<CertificateViewModel> installedCertificates { get; } = new();

        public MainWindowViewModel(CertificateManager certificateManager) {
            this.certificateManager = certificateManager;

            manageCertificatesCommand = new DelegateCommand(manageCertificates);
            saveCommand               = new DelegateCommand(save, isDirty);
            refreshCommand            = new DelegateCommand(refresh);

            refresh();
        }

        public void setActive(CertificateViewModel selectedItem) {
            CertificateViewModel? oldActiveCertificate = activeCertificateViewModel;
            if (oldActiveCertificate is not null) {
                oldActiveCertificate.isActive = false;
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
                    saveCommand.RaiseCanExecuteChanged();
                    MessageBox.Show($"Remote Desktop Services are now using the \"{selectedCertificate.name}\" certificate, which expires on {selectedCertificate.expirationDate:d}.",
                        "Connection updated", MessageBoxButton.OK, MessageBoxImage.Information);
                } catch (CimException e) {
                    MessageBox.Show(e.Message, "Failed to change certificate", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            } else {
                MessageBox.Show("No certificate selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool isDirty() {
            return !certificateManager.activeTerminalServicesCertificate.Equals(activeCertificateViewModel?.certificate);
        }

        private void refresh() {
            Certificate activeCertificate = certificateManager.activeTerminalServicesCertificate;

            foreach (CertificateViewModel installedCertificate in installedCertificates) {
                installedCertificate.PropertyChanged -= onCertificateViewModelChanged;
            }

            installedCertificates.Clear();
            installedCertificates.AddRange(certificateManager.installedCertificates
                .Select(certificate => {
                    var certificateViewModel = new CertificateViewModel(certificate, certificateManager) { isActive = certificate == activeCertificate };
                    certificateViewModel.PropertyChanged += onCertificateViewModelChanged;
                    return certificateViewModel;
                }));

            saveCommand.RaiseCanExecuteChanged();
        }

        private void onCertificateViewModelChanged(object sender, PropertyChangedEventArgs args) {
            if (args.PropertyName == nameof(CertificateViewModel.isActive)) {
                saveCommand.RaiseCanExecuteChanged();
            }
        }

    }

}