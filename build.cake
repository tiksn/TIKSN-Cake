#addin nuget:?package=Cake.Git
#r ".\\TIKSN.Cake.Core\\bin\\Debug\\netstandard2.0\\TIKSN.Cake.Core.dll"
#r ".\\TIKSN.Cake.Addin\\bin\\Debug\\netstandard2.0\\TIKSN.Cake.Addin.dll"

DirectoryPath gitRootDir;

Setup(context =>
{
    gitRootDir = GitFindRootFromPath(".");
});

Teardown(context =>
{
    // Executed AFTER the last task.
});

Task("A")
    .Does(() =>
{
    Hello("TIKSN");
});

RunTarget("A");
