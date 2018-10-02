using Microsoft.Extensions.DependencyInjection;
using TIKSN.Cake.Core.Services;
using Xunit;

namespace TIKSN.Cake.Core.Tests
{
    public class VersioningServiceTests
    {
        [Fact]
        public void CanResolveService() => DependencyInjectionHelper.CreateServiceProvider().GetRequiredService<IVersioningService>();
    }
}