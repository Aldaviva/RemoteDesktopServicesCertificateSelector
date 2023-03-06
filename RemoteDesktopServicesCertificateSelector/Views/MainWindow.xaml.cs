#nullable enable

using System.Windows;
using System.Windows.Input;
using Dark.Net;
using RemoteDesktopServicesCertificateSelector.ViewModels;

namespace RemoteDesktopServicesCertificateSelector.Views; 

public partial class MainWindow {

    private MainWindowViewModel viewModel => (MainWindowViewModel) DataContext;

    public MainWindow() {
        InitializeComponent();
        DarkNet.Instance.SetWindowThemeWpf(this, Theme.Auto);
    }

    private void onWindowLoaded(object sender, RoutedEventArgs e) {
        dataGrid.Focus();
        dataGrid.CurrentCell = dataGrid.SelectedCells[0];
    }

    private void onDataGridKeyUp(object _, KeyEventArgs args) {
        if (args.Key == Key.Space) {
            args.Handled = true;
            viewModel.setActive((CertificateViewModel) dataGrid.SelectedItem);
        }
    }

}