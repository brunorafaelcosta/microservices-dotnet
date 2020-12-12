Write-Host "Executing the pre-build script..."

cd ../../src/Services/

Remove-Item dotnet-restore.Development.Dockerfile.tar -ErrorAction Ignore
Remove-Item ../dotnet-restore.Development.Dockerfile -Recurse -ErrorAction Ignore
ROBOCOPY . ../dotnet-restore.Development.Dockerfile *.csproj /S
tar -cvf dotnet-restore.Development.Dockerfile.tar -C ../dotnet-restore.Development.Dockerfile .
Remove-Item ../dotnet-restore.Development.Dockerfile -Recurse -ErrorAction Ignore

Write-Host "Successfully executed the pre-build script..."

Write-Host -NoNewLine "Press any key to continue..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
