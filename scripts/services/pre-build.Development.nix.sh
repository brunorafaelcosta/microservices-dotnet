#!/bin/bash

echo "Executing the pre-build script..."

cd ../../src/Services/

rm -f dotnet-restore.Development.Dockerfile.tar
find . -name "*.csproj" -o -name "microservices-dotnet-services.sln" -o -name "NuGet.Config" | sort | tar cvf "dotnet-restore.Development.Dockerfile.tar" -T - 2> /dev/null

echo "Successfully executed the pre-build script..."