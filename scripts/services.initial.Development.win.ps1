$thisPath = $MyInvocation.MyCommand.Path
$thisPath = Split-Path $thisPath

# Identity
cd ../src/Services/Identity/
## Generate self signed certificate for SigningCredential
Write-Host 'Identity - Going to generate the SigningCredential certificate!';
cd Services.Identity.STS/Certificates/
Remove-Item "development.signingcredential1.pfx" -ErrorAction Ignore
openssl req -x509 -newkey rsa:4096 -sha256 -nodes -keyout "development.signingcredential1.key" -out "development.signingcredential1.crt" -subj "/CN=localhost" -days 3650
openssl pkcs12 -export -out "development.signingcredential1.pfx" -inkey "development.signingcredential1.key" -in "development.signingcredential1.crt" -certfile "development.signingcredential1.crt"
cd ../../
Write-Host 'Identity - Sucessfully generated the SigningCredential certificate!';

cd $thisPath

# Pre-build script
Write-Host 'Executing the pre-build script...';
./services.pre-build.Development.win.ps1
Write-Host 'Sucessfully executed the pre-build script...';

Write-Host 'Press any key to continue...';
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');