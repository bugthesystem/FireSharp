# https://www.appveyor.com/docs/environment-variables/

# Script static variables
$buildDir = $env:APPVEYOR_BUILD_FOLDER # e.g. C:\projects\rn-common\
$buildNumber = $env:APPVEYOR_BUILD_VERSION # e.g. 1.0.17

# C:\projects\rn-common\src\Rn.Common
$projectDir = $buildDir + "\FireSharp.Core";
# C:\projects\rn-common\src\Rn.Common\Rn.Common.csproj
$projectFile = $projectDir + "\FireSharp.Core.csproj";
# C:\projects\rn-common\src\Rn.CommonTests
$testDir = $buildDir + "\FireSharp.Tests.Core";
# C:\projects\rn-common\src\Rn.Common\Rn.Common.x.x.x.nupkg
$nugetFile = $projectDir + "\FireSharp.Core." + $buildNumber + ".nupkg";


# Display .Net Core version
Write-Host "Checking .NET Core version" -ForegroundColor Green
& dotnet --version

# Restore the main project
Write-Host "Restoring project" -ForegroundColor Green
& dotnet restore $projectFile --verbosity m

# Publish the project
Write-Host "Publishing project" -ForegroundColor Green
& dotnet publish $projectFile

# Discover and run tests
Write-Host "Running tests" -ForegroundColor Green
cd $testDir
& dotnet restore FireSharp.Tests.Core.csproj --verbosity m
$testOutput = & dotnet test | Out-String
Write-Host $testOutput

# Ensure that the tests passed
if ($testOutput.Contains("Test Run Successful.") -eq $False) {
  Write-Host "Build failed!";
  Exit;
}

# Generate a NuGet package for publishing
Write-Host "Generating NuGet Package" -ForegroundColor Green
cd $projectDir
& dotnet pack -c Release /p:PackageVersion=$buildNumber -o $projectDir

# Save generated artifacts
Write-Host "Saving Artifacts" -ForegroundColor Green
Push-AppveyorArtifact $nugetFile

# Done
Write-Host "Done!" -ForegroundColor Green