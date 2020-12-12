#!/bin/bash

echo "Executing the initial script..."

thisPath="$( cd "$(dirname "$0")" >/dev/null 2>&1 ; pwd -P )"

# Identity
cd ../../src/Services/Identity/
## Generate self signed certificate for SigningCredential
echo "Identity - Going to generate the SigningCredential certificate!"
cd Services.Identity.STS/Certificates/
rm -f "development.signingcredential.pfx"
openssl req -x509 -newkey rsa:4096 -sha256 -nodes -keyout "development.signingcredential.key" -out "development.signingcredential.crt" -subj "/CN=localhost" -days 3650
openssl pkcs12 -export -out "development.signingcredential.pfx" -inkey "development.signingcredential.key" -in "development.signingcredential.crt" -certfile "development.signingcredential.crt"
rm -f "development.signingcredential.key"
rm -f "development.signingcredential.crt"
cd ../../
echo "Identity - Successfully generated the SigningCredential certificate!"

cd $thisPath

# Pre-build script
./pre-build.Development.nix.sh

cd $thisPath

echo "Successfully executed the initial script..."
