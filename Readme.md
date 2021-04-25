<img src="https://raw.githubusercontent.com/Aldaviva/RemoteDesktopServicesCertificateSelector/master/RemoteDesktopServicesCertificateSelector/Resources/terminalServicesManagement.ico" width="32" /> Remote Desktop Services Certificate Selector
===

This GUI program lets you choose the certificate to use for your Remote Desktop Services connection.

In Windows Server 2008 R2 and earlier, this functionality was available in the Remote Desktop Session Host Configuration (`tsconfig.msc`), but it was removed in Windows Server 2012 and later, which means you can only configure it programmatically using WMI.

![screenshot](https://i.imgur.com/Z9jFnCY.png)

## Requirements

- Windows (tested on Server 2019)
- [.NET Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework) or later

## Installation

1. Download the latest EXE file from [Releases](https://github.com/Aldaviva/RemoteDesktopServicesCertificateSelector/releases).

It's a portable application. You can save it, run it, and then delete it when you're done. It won't leave any files or registry values behind.

## Usage

1. Install your new certificate into Certificates (Local Computer) › Personal › Certificates. You can open this certificate store by clicking **![certs](https://raw.githubusercontent.com/Aldaviva/RemoteDesktopServicesCertificateSelector/master/RemoteDesktopServicesCertificateSelector/Resources/certs.png) Manage Local Computer Certificates** in this program.
1. Run `RemoteDesktopServicesCertificateSelector.exe`.
1. Click the radio button for the certificate you want to use on your RDP connections.
1. Click **![save](https://raw.githubusercontent.com/Aldaviva/RemoteDesktopServicesCertificateSelector/master/RemoteDesktopServicesCertificateSelector/Resources/save.ico) Apply**.

New connections to your `RDP-tcp` listener will now use the new certificate.

You can copy a certificate's thumbprint by right-clicking on a row and selecting **![copy](https://raw.githubusercontent.com/Aldaviva/RemoteDesktopServicesCertificateSelector/master/RemoteDesktopServicesCertificateSelector/Resources/copy.ico) Copy thumbprint (SHA-1)**.