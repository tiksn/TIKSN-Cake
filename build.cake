#addin "Cake.Http"
#addin "Cake.Json"
#addin "Cake.ExtendedNuGet"
#addin nuget:?package=Newtonsoft.Json&version=9.0.1
#addin nuget:?package=NuGet.Core&version=2.14.0
#addin nuget:?package=NuGet.Versioning&version=4.6.2
#tool "nuget:?package=xunit.runner.console"
#addin nuget:?package=Cake.Git
#addin nuget:?package=TIKSN-Cake&loaddependencies=true

var target = Argument("target", "Publish");
var solution = "TIKSN Cake.sln";
var nuspec = "TIKSN-Cake.nuspec";
var nextVersionString = "";

using System;
using System.Linq;
using NuGet.Versioning;
DirectoryPath buildArtifactsDir;

Setup(context =>
{
    SetTrashParentDirectory(GitFindRootFromPath("."));
});

Teardown(context =>
{
    // Executed AFTER the last task.
});

Task("Publish")
  .Description("Publish NuGet package.")
  .IsDependentOn("Pack")
  .Does(() =>
{
 var package = string.Format("tools/TIKSN-Cake.{0}.nupkg", nextVersionString);

 NuGetPush(package, new NuGetPushSettings {
     Source = "nuget.org",
     ApiKey = EnvironmentVariable("TIKSN-Cake-ApiKey")
 });
});

Task("Pack")
  .Description("Pack NuGet package.")
  .IsDependentOn("Build")
  .IsDependentOn("EstimateNextVersion")
  //.IsDependentOn("Test")
  .Does(() =>
{
  var nuGetPackSettings = new NuGetPackSettings {
    Version = nextVersionString,
    BasePath = buildArtifactsDir,
    OutputDirectory = "tools" // GetTrashDirectory()
    };

  NuGetPack(nuspec, nuGetPackSettings);
});

Task("Test")
  .IsDependentOn("Build")
  .Does(() =>
{
  XUnit2("TIKSN.Framework.Tests/bin/Release/TIKSN.Framework.Tests.dll");
  XUnit2("UnitTests/bin/Release/netstandard2.0/UnitTests.dll");
});

Task("Build")
  .IsDependentOn("Clean")
  .IsDependentOn("Restore")
  .Does(() =>
{
  buildArtifactsDir = CreateTrashSubDirectory("artifacts");

  MSBuild(solution, configurator =>
    configurator.SetConfiguration("Release")
        .SetVerbosity(Verbosity.Minimal)
        .UseToolVersion(MSBuildToolVersion.VS2017)
        .SetMSBuildPlatform(MSBuildPlatform.x64)
        .SetPlatformTarget(PlatformTarget.MSIL)
        .WithProperty("OutDir", buildArtifactsDir.FullPath)
        //.WithTarget("Rebuild")
        );
});

Task("EstimateNextVersion")
  .Description("Estimate next version.")
  .Does(() =>
{
  var packageList = NuGetList("TIKSN-Cake", new NuGetListSettings {
      AllVersions = false,
      Prerelease = false
      });
  var latestPackage = packageList.Single();
  var latestPackageNuGetVersion = new NuGetVersion(latestPackage.Version);
  var nextVersion = new NuGetVersion(latestPackageNuGetVersion.Version.Major,latestPackageNuGetVersion.Version.Minor,latestPackageNuGetVersion.Version.Build + 1);
  nextVersionString = nextVersion.ToString();
  Information("Next version estimated to be " + nextVersionString);
});

Task("Restore")
  .Description("Restores packages.")
  .Does(() =>
{
  NuGetRestore(solution);
});

Task("Clean")
  .Description("Cleans all directories that are used during the build process.")
  .Does(() =>
{
  CleanDirectories("**/bin/**");
  CleanDirectories("**/obj/**");
});

RunTarget(target);