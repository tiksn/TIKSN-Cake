using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Microsoft.Extensions.DependencyInjection;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public static void GenerateLocalizationKeys(
            this ICakeContext ctx,
            string @namespace,
            string @class,
            DirectoryPath outputDirectory,
            params FilePath[] resxFiles)
        {
            var localizationKeysGenerator = serviceProvider.GetRequiredService<ILocalizationKeysGenerator>();
            localizationKeysGenerator.GenerateLocalizationKeys(
                new LoggerAdapter(ctx.Log),
                @namespace,
                @class,
                outputDirectory.FullPath,
                resxFiles.Select(f => f.FullPath).ToArray());
        }

        [CakeMethodAlias]
        public static Versioning.Version GetNextEstimatedVersion(this ICakeContext ctx)
        {
            var versioningService = serviceProvider.GetRequiredService<IVersioningService>();
            return versioningService.GetNextVersion(
                new LoggerAdapter(ctx.Log),
                ctx.Arguments.GetArgument("next_version"),
                ctx.Arguments.GetArgument("next_version_strategy"));
        }

        [CakeMethodAlias]
        public static DirectoryPath GetTrashDirectory(this ICakeContext ctx)
        {
            var trashFolderServices = serviceProvider.GetRequiredService<ITrashFolderServices>();
            var trashFolderPath = trashFolderServices.GetTrashFolder(new LoggerAdapter(ctx.Log));

            return new DirectoryPath(trashFolderPath);
        }

        [CakeMethodAlias]
        public static void SetPublishedVersions(this ICakeContext ctx, IEnumerable<NuGetVersion> versions)
        {
            var versioningService = serviceProvider.GetRequiredService<IVersioningService>();
            versioningService.SetVersions(new LoggerAdapter(ctx.Log), versions);
        }

        [CakeMethodAlias]
        public static void SetPublishedVersions(this ICakeContext ctx, IEnumerable<SemanticVersion> versions)
        {
            var versioningService = serviceProvider.GetRequiredService<IVersioningService>();
            versioningService.SetVersions(new LoggerAdapter(ctx.Log), versions);
        }

        [CakeMethodAlias]
        public static void SetPublishedVersions(this ICakeContext ctx, IEnumerable<Versioning.Version> versions)
        {
            var versioningService = serviceProvider.GetRequiredService<IVersioningService>();
            versioningService.SetVersions(new LoggerAdapter(ctx.Log), versions);
        }

        [CakeMethodAlias]
        public static void SetPublishedVersions(this ICakeContext ctx, IEnumerable<System.Version> versions)
        {
            var versioningService = serviceProvider.GetRequiredService<IVersioningService>();
            versioningService.SetVersions(new LoggerAdapter(ctx.Log), versions);
        }

        [CakeMethodAlias]
        public static void SetTrashParentDirectory(this ICakeContext ctx, DirectoryPath rootDirectoryPath)
        {
            var trashFolderServices = serviceProvider.GetRequiredService<ITrashFolderServices>();
            trashFolderServices.SetTrashParentFolder(new LoggerAdapter(ctx.Log), rootDirectoryPath.FullPath);
        }
    }
}