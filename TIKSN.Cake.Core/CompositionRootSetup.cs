using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        }

        protected override void ConfigureOptions(IServiceCollection services, IConfigurationRoot configuration)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
        }
    }
}