# Certificate

To install a certificate to let Front running https in container, follow the steps below:

## Generate a PFX file from an Existing Certificate

1. Generate a certificate and key file with openssl

Download SSL here: https://slproweb.com/products/Win32OpenSSL.html

The commands below demonstrate examples of how to create a .pfx/.p12 file in the command line using OpenSSL:

```
openssl pkcs12 -export -out certificate.pfx -inkey private.key -in certificate.crt
```

Breaking down the command:

* openssl – the command for executing OpenSSL
* pkcs12 – the file utility for PKCS#12 files in OpenSSL
* -export -out certificate.pfx – export and save the PFX file as certificate.pfx
* -inkey privateKey.key – use the private key file privateKey.key as the private key to combine with the certificate.
* -in certificate.crt – use certificate.crt as the certificate the private key will be combined with.
* -certfile more.crt – This is optional, this is if you have any additional certificates you would like to include in the PFX file.

2. Copy the certificate.pfx file to the container

The container has two volumes for certificates mounted, we use this one here:

```
C:\Users\{user}\.aspnet\https	/https	Read only	
```

Copy the certificate.pfx file to the volume. Replace {user} with your username. Check the *docker-compose.yml* file for the exact path.

- ASPNETCORE_Kestrel__Certificates__Default__Password=cryptickey
- ASPNETCORE_Kestrel__Certificates__Default__Path=/https/TheDashboard.Frontend.pfx

## Generate a Certificate for Localhost

### Generate a private key

```
openssl genpkey -algorithm RSA -out private_key.pem
```

### Generate a self-signed certificate

```
openssl req -new -x509 -key private_key.pem -out certificate.pem -days 365
```

### Convert the certificate to PFX format

```
openssl pkcs12 -export -out TheDashboard.Frontend.pfx -inkey private_key.pem -in certificate.pem
```

### Copy the *TheDashboard.Frontend.pfx* file to the container

The container has two volumes for certificates mounted, we use this one here:

```
C:\Users\{user}\.aspnet\https	/https	Read only	
```

Copy the *TheDashboard.Frontend.pfx* file to the volume. Replace {user} with your username. Check the *docker-compose.yml* file for the exact path.

- ASPNETCORE_Kestrel__Certificates__Default__Password=cryptickey
- ASPNETCORE_Kestrel__Certificates__Default__Path=/https/TheDashboard.Frontend.pfx

## Trust the Certificate

### Windows

1. Open the certificate file. Windows will open the Certificate Import Wizard.
1. Select Local Machine and click Next.
1. Select Place all certificates in the following store and click Browse.
1. Select Trusted Root Certification Authorities and click OK.
1. Click Next, then click Finish.
1. Click Yes to install the certificate.
1. Click OK to confirm the certificate was installed.

### Mac

1. Open Keychain Access.
1. Select System in the left sidebar.
1. Select Certificates in the category sidebar.
1. Select File > Import Items.
1. Select the certificate file and click Open.
1. Enter your password to allow this.
1. Double-click the certificate.
1. Expand Trust.
1. Change the When using this certificate setting to Always Trust.
1. Close the certificate window.
1. Enter your password to allow this.
1. Restart Chrome.


