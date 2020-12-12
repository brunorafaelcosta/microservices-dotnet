Write-Host "Executing the initial script..."

$thisPath = $MyInvocation.MyCommand.Path
$thisPath = Split-Path $thisPath

# Identity
cd ../../src/Services/Identity/
## Generate self signed certificate for SigningCredential
Write-Host "Identity - Going to generate the SigningCredential certificate!"
cd Services.Identity.STS/Certificates/
Remove-Item "development.signingcredential.pfx" -ErrorAction Ignore
openssl req -x509 -newkey rsa:4096 -sha256 -nodes -keyout "development.signingcredential.key" -out "development.signingcredential.crt" -subj "/CN=localhost" -days 3650
openssl pkcs12 -export -out "development.signingcredential.pfx" -inkey "development.signingcredential.key" -in "development.signingcredential.crt" -certfile "development.signingcredential.crt"
Remove-Item "development.signingcredential.key" -ErrorAction Ignore
Remove-Item "development.signingcredential.crt" -ErrorAction Ignore
cd ../../
Write-Host "Identity - Successfully generated the SigningCredential certificate!"

cd $thisPath

# Pre-build script
./pre-build.Development.win.ps1

cd $thisPath

Write-Host "Successfully executed the initial script..."

Write-Host "Press any key to continue..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
