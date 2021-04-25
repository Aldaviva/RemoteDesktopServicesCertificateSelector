using System.Windows;
using System.Windows.Input;
using RemoteDesktopServicesCertificateSelector.ViewModels;

#nullable enable

namespace RemoteDesktopServicesCertificateSelector.Views {

    public partial class MainWindow {

        private MainWindowViewModel viewModel => (MainWindowViewModel) DataContext;

        public MainWindow() {
            InitializeComponent();
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

}