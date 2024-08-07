# Define the certificate parameters
$certName = "CN=MySelfSignedCert"
$certPath = "Cert:\LocalMachine\My"
$exportPath = ".\TestCert.pfx"
$certPassword = ConvertTo-SecureString -String "Test" -Force -AsPlainText

# Create the self-signed certificate
$cert = New-SelfSignedCertificate -Subject $certName -CertStoreLocation $certPath -KeyExportPolicy Exportable -KeySpec Signature -NotAfter (Get-Date).AddYears(5)

# Export the certificate to a PFX file
Export-PfxCertificate -Cert $cert -FilePath $exportPath -Password $certPassword

Write-Host "Certificate created and exported successfully to $exportPath"
