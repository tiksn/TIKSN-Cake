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
var nugetPackageId = "TIKSN-Cake";

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
 var package = string.Format("{0}/{1}.{2}.nupkg", GetTrashDirectory(), nugetPackageId, (NuGetVersion)GetNextEstimatedVersion());

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
    Version = GetNextEstimatedVersion().ToString(),
    BasePath = buildArtifactsDir,
    OutputDirectory = GetTrashDirectory()
    };

  NuGetPack(nuspec, nuGetPackSettings);
});

Task("Test")
  .IsDependentOn("Build")
  .Does(() =>
{
  XUnit2(new [] {
    buildArtifactsDir.CombineWithFilePath("TIKSN.Cake.Core.Tests.dll")
     },
     new XUnit2Settings {
        Parallelism = ParallelismOption.All,
        HtmlReport = true,
        XmlReport = true,
        NoAppDomain = true,
        OutputDirectory = CreateTrashSubDirectory("test-results")
    });
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
  var packageList = NuGetList(nugetPackageId, new NuGetListSettings {
      AllVersions = false,
      Prerelease = true
      });
  SetPublishedVersions(packageList.Select(v => new NuGetVersion(v.Version)));
  Information("Next version estimated to be " + GetNextEstimatedVersion());
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