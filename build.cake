#addin nuget:?package=Cake.Git
#addin nuget:?package=TIKSN-Framework&loaddependencies=true


#r "./TIKSN.Cake.Core/bin/Debug/netstandard2.0/TIKSN.Cake.Core.dll"
#r "./TIKSN.Cake.Addin/bin/Debug/netstandard2.0/TIKSN.Cake.Addin.dll"

Setup(context =>
{
    SetTrashParentDirectory(GitFindRootFromPath("."));
});

Teardown(context =>
{
    // Executed AFTER the last task.
});

Task("A")
    .Does(() =>
{
    var artifacts = CreateTrashSubDirectory("artifacts");
});

RunTarget("A");
