<img src="https://raw.githubusercontent.com/Aldaviva/RemoteDesktopServicesCertificateSelector/master/RemoteDesktopServicesCertificateSelector/Resources/terminalServicesManagement.ico" width="32" /> Remote Desktop Services Certificate Selector
===

This GUI program lets you choose the certificate to use for your Remote Desktop Services connection.

In Windows Server 2008 R2 and earlier, this functionality was available in the Remote Desktop Session Host Configuration (`tsconfig.msc`), but it was removed in Windows Server 2012 and later, which means you can only [configure it programmatically using WMI](https://serverfault.com/a/444287/227008).

![screenshot](https://i.imgur.com/SmXGPZR.png)

<!-- MarkdownTOC autolink="true" bracket="round" autoanchor="true" levels="1,2,3" style="ordered" -->

1. [Requirements](#requirements)
1. [Installation](#installation)
1. [Certificate Conversion](#certificate-conversion)
1. [Usage](#usage)
1. [Validation](#validation)

<!-- /MarkdownTOC -->

<a id="requirements"></a>
## Requirements

- Windows (tested on Server 2019)
- [.NET Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework) or later

<a id="installation"></a>
## Installation

1. Download the latest EXE file from [Releases](https://github.com/Aldaviva/RemoteDesktopServicesCertificateSelector/releases).

It's a portable application. You can save it, run it, and then delete it when you're done. It won't leave any files or registry values behind.

<a id="certificate-conversion"></a>
## Certificate Conversion
The private key and public certificate is required for servers like Remote Desktop Services. These must be imported into a Windows certificate store using PCKS#12 format, which uses the PFX file extension.

If you have an X.509 certificate that you want to import, like a PEM-encoded RSA keypair, you will first need to temporarily convert it to PKCS#12.

```sh
openssl pkcs12 -in "mypubliccert.pem.crt" -inkey "myprivatekey.pem.key" -out "mycertandkey.pfx" -export
```

This PFX file is the one to import into Certificates, not the CRT file. Be aware that the Certificate Import Wizard defaults to only showing X.509 keys (CER, CRT), so it's easy to accidentally import only the public key and therefore be unable to use it for your server. Be sure to change the file type dropdown in the Open dialog box to Personal Information Exchange so that your PFX file is shown.

After importing the PFX file, you can delete it from disk.

<a id="usage"></a>
## Usage

1. Run `RemoteDesktopServicesCertificateSelector.exe`.
1. If you haven't already done so, install your new certificate into Certificates (Local Computer) › Personal › Certificates.
    - You can open this certificate store by clicking **![certs](https://raw.githubusercontent.com/Aldaviva/RemoteDesktopServicesCertificateSelector/master/RemoteDesktopServicesCertificateSelector/Resources/certs.ico) Manage Local Computer Certificates**.
    - Once it's installed, click **![refresh](https://raw.githubusercontent.com/Aldaviva/RemoteDesktopServicesCertificateSelector/master/RemoteDesktopServicesCertificateSelector/Resources/refresh.png) Refresh** in this program to show the newly-installed certificate.
1. Click the radio button for the certificate you want to use on your RDP connections.
1. Click **![save](https://raw.githubusercontent.com/Aldaviva/RemoteDesktopServicesCertificateSelector/master/RemoteDesktopServicesCertificateSelector/Resources/save.ico) Apply**.

New connections to your `RDP-tcp` listener will now use the new certificate. This change takes effect immediately for any new connections; you don't need to restart any services or your computer.

You can view a certificate or copy its thumbprint by right-clicking on a row.

<a id="validation"></a>
## Validation

To test the new certificate, you can reconnect using `mstsc.exe` and click the 🔒 button in the fullscreen toolbar.

You can also test it with `openssl`:
```sh
echo | openssl s_client -connect myserver.com:3389 2>/dev/null | openssl x509 -noout -text
```