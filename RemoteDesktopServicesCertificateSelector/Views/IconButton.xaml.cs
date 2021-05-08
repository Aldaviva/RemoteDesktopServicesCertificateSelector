using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

#nullable enable

namespace RemoteDesktopServicesCertificateSelector.Views {

    [ContentProperty(nameof(Text))]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public partial class IconButton {

        public static readonly DependencyProperty TextProperty    = DependencyProperty.Register(nameof(Text), typeof(string), typeof(IconButton));
        public static readonly DependencyProperty IconProperty    = DependencyProperty.Register(nameof(Icon), typeof(ImageSource), typeof(IconButton));
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(IconButton));

        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent(nameof(Click), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(IconButton));

        public string Text {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public ImageSource Icon {
            get => (ImageSource) GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public ICommand Command {
            get => (ICommand) GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public IconButton() {
            InitializeComponent();
        }

        private void onButtonClick(object sender, RoutedEventArgs e) {
            RaiseEvent(new RoutedEventArgs(ClickEvent, this));
        }

        public event RoutedEventHandler Click {
            add => AddHandler(ClickEvent, value);
            remove => RemoveHandler(ClickEvent, value);
        }

    }

}