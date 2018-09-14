using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Microsoft.Extensions.DependencyInjection;
using System;
using TIKSN.Cake.Core;
using TIKSN.Cake.Core.Services;

namespace TIKSN.Cake.Addin
{
    [CakeAliasCategory("TIKSN.Cake")]
    [CakeNamespaceImport("TIKSN.Cake.Addin")]
    public static class AddinAliases
    {
        private static readonly IServiceProvider serviceProvider;

        static AddinAliases()
        {
            var configurationRootSetup = new ConfigurationRootSetup();
            var configurationRoot = configurationRootSetup.GetConfigurationRoot();
            var compositionRootSetup = new CompositionRootSetup(configurationRoot);
            serviceProvider = compositionRootSetup.CreateServiceProvider();
        }

        [CakeMethodAlias]
        public static DirectoryPath CreateTrashSubDirectory(this ICakeContext ctx, string subdirectoryName)
        {
            var trashFolderServices = serviceProvider.GetRequiredService<ITrashFolderServices>();
            var subdirectoryPath = trashFolderServices.CreateTrashSubFolder(new LoggerAdapter(ctx.Log), subdirectoryName);

            return new DirectoryPath(subdirectoryPath);
        }

        [CakeMethodAlias]
        public static void SetTrashParentDirectory(this ICakeContext ctx, DirectoryPath rootDirectoryPath)
        {
            var trashFolderServices = serviceProvider.GetRequiredService<ITrashFolderServices>();
            trashFolderServices.SetTrashParentFolder(new LoggerAdapter(ctx.Log), rootDirectoryPath.FullPath);
        }
    }
}