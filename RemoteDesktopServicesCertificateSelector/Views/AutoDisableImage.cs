#nullable enable

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RemoteDesktopServicesCertificateSelector.Views;

/// <summary>
/// https://www.renebergelt.de/blog/2019/10/automatically-grayscale-images-on-disabled-wpf-buttons/
/// </summary>
internal class AutoDisableImage: Image {

    protected bool isGrayscaled => Source is FormatConvertedBitmap;

    static AutoDisableImage() {
        // Override the metadata of the IsEnabled and Source properties to be notified of changes
        IsEnabledProperty.OverrideMetadata(typeof(AutoDisableImage), new FrameworkPropertyMetadata(true, OnAutoDisableImagePropertyChanged));
        SourceProperty.OverrideMetadata(typeof(AutoDisableImage), new FrameworkPropertyMetadata(null, OnAutoDisableImagePropertyChanged));
    }

    /// <summary>
    /// Called when AutoDisableImage's IsEnabled or Source property values changed
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
    private static void OnAutoDisableImagePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs args) {
        if (source is AutoDisableImage src && src.IsEnabled == src.isGrayscaled) {
            src.updateImage();
        }
    }

    private void updateImage() {
        if (IsEnabled && isGrayscaled && Source is FormatConvertedBitmap source) {
            Source      = source.Source;
            OpacityMask = null;
        } else if (!isGrayscaled && Source is BitmapSource bitmapImage) {
            Source      = new FormatConvertedBitmap(bitmapImage, PixelFormats.Gray8, null, 0);
            OpacityMask = new ImageBrush(bitmapImage);
        }
    }

}