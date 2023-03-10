#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using RemoteDesktopServicesCertificateSelector.Data;
using RemoteDesktopServicesCertificateSelector.Managers;

namespace RemoteDesktopServicesCertificateSelector.ViewModels.Samples;

public class SampleMainWindowViewModel: IMainWindowViewModel {

    private readonly CertificateManager certificateManager = new FakeCertificateManager();

    public SampleMainWindowViewModel() {
        installedCertificates = new ObservableCollection<CertificateViewModel>(new[] {
            new CertificateViewModel(new Certificate("abc", "Name", "Issuer", DateTime.Now.AddDays(1), false), certificateManager),
            new CertificateViewModel(new Certificate("def", "Name 2", "Issuer 2", DateTime.Now.AddDays(2), false), certificateManager),
            new CertificateViewModel(new Certificate("ghi", "Name 3", "Issuer 3", DateTime.Now.AddDays(-1), false), certificateManager)
        });
    }

    public CertificateViewModel activeCertificateViewModel => installedCertificates.First();
    public DelegateCommand manageCertificatesCommand { get; } = null!;
    public DelegateCommand saveCommand { get; } = null!;
    public DelegateCommand refreshCommand { get; } = null!;
    public string title { get; } = "My Title";

    public ObservableCollection<CertificateViewModel> installedCertificates { get; }

    public void setActive(CertificateViewModel selectedItem) { }

#pragma warning disable CS0067 // it's just a sample class for XAML authoring
    public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore CS0067

    private class FakeCertificateManager: CertificateManager {

        public IEnumerable<Certificate> installedCertificates { get; } = Array.Empty<Certificate>();
        public Certificate activeTerminalServicesCertificate { get; set; } = new("fake");

        public Task launchCertificateManagementConsole() {
            return Task.CompletedTask;
        }

        public void openCertificate(Certificate certificate) { }

    }

}