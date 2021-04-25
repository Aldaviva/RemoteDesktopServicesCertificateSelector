#nullable enable

using System.Collections.ObjectModel;
using System.Windows;
using CertificateUpdater.Data;
using CertificateUpdater.Managers;
using Microsoft.Management.Infrastructure;
using Prism.Commands;
using Prism.Mvvm;

namespace CertificateUpdater.ViewModels {

    public class MainWindowViewModel: BindableBase {

        private readonly CertificateManager certificateManager;

        public MainWindowViewModel(CertificateManager certificateManager) {
            this.certificateManager = certificateManager;
            installedCertificates.AddRange(certificateManager.installedCertificates);
            selectedCertificate = certificateManager.activeTerminalServicesCertificate;

            saveCommand               = new DelegateCommand(save);
            manageCertificatesCommand = new DelegateCommand(manageCertificates);
        }

        public string title { get; } = "Remote Desktop Connection Certificate";

        public ObservableCollection<Certificate> installedCertificates { get; } = new();

        private Certificate? _selectedCertificate;

        public Certificate? selectedCertificate {
            get => _selectedCertificate;
            set => SetProperty(ref _selectedCertificate, value);
        }

        public DelegateCommand manageCertificatesCommand { get; }

        private void manageCertificates() {
            certificateManager.launchCertificateManagementConsole();
        }

        public DelegateCommand saveCommand { get; }

        private void save() {
            if (selectedCertificate != null) {
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