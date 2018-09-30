using System;

namespace TIKSN.Cake.Core.Tests
{
    public static class DependencyInjectionHelper
    {
        public static IServiceProvider CreateServiceProvider() => new CompositionRootSetup(new ConfigurationRootSetup().GetConfigurationRoot()).CreateServiceProvider();
    }
}