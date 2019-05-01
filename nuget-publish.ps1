$version = Read-Host -Prompt "Enter version to publish"
$nugetKey = Read-Host -Prompt "Enter nuget key"

Remove-Item -Path ".\src\Vcr\bin\Release" -Force -Recurse
& dotnet build --configuration "Release"
& dotnet test --configuration "Release"
Push-Location ".\src\Vcr"
& dotnet pack --configuration "Release" --no-build --no-restore --include-symbols
& dotnet nuget push ".\bin\Release\VCR.net.$version.nupkg" -k $nugetKey -s https://api.nuget.org/v3/index.json
Pop-Location