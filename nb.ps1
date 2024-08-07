
$destinationFolderPath = ".\JwtGenerator"

if (Test-Path -Path $destinationFolderPath) {
    Remove-Item -Path $destinationFolderPath -Recurse -Force
    Write-Host "Folder deleted successfully."
}

dotnet new console -o JwtGenerator --force

# Define the source and destination file paths
$sourceFilePath = ".\Program.cs"
$destinationFilePath = ".\JwtGenerator\Program.cs"

# Replace the destination file with the source file
Copy-Item -Path $sourceFilePath -Destination $destinationFilePath -Force

Set-Location JwtGenerator
dotnet add package System.IdentityModel.Tokens.Jwt
dotnet add package Microsoft.IdentityModel.Tokens

dotnet build

$cert = "..\TestCert.pfx"
if (Test-Path -Path $cert) {
    Copy-Item $cert .\TestCert.pfx
}
else{
    ..\Create-SelfSignedCert.ps1
}

dotnet run

Set-Location ".."
