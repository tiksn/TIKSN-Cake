using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;
using System;
using TIKSN.Cake.Core;

namespace TIKSN.Cake.Addin
{
    [CakeAliasCategory("TIKSN.Cake")]
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
        public static void Hello(this ICakeContext ctx, string name)
        {
            ctx.Log.Information("Hello " + name);
        }
    }
}