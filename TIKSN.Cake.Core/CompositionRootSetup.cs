using Autofac;
using IdGen;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TIKSN.Cake.Core.Services;
using TIKSN.DependencyInjection;

namespace TIKSN.Cake.Core
{
    public class CompositionRootSetup : AutofacCompositionRootSetupBase
    {
        public CompositionRootSetup(IConfigurationRoot configurationRoot) : base(configurationRoot)
        {
        }

        protected override void ConfigureContainerBuilder(ContainerBuilder builder)
        {
            builder.RegisterType<TrashFolderServices>().As<ITrashFolderServices>().SingleInstance();
            builder.RegisterType<LocalizationKeysGenerator>().As<ILocalizationKeysGenerator>().SingleInstance();
            builder.RegisterType<VersioningService>().As<IVersioningService>().SingleInstance();
            RegisterIdGenerator(builder);
        }

        protected override void ConfigureOptions(IServiceCollection services, IConfigurationRoot configuration)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
        }

        private static void RegisterIdGenerator(ContainerBuilder builder)
        {
            var epoch = new DateTime(2019, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var mc = new MaskConfig(47, 0, 16);
            var generator = new IdGenerator(0, epoch, mc);
            builder.RegisterInstance(generator).As<IIdGenerator<long>>();
        }
    }
}