using FluentAssertions;
using IdGen;
using Microsoft.Extensions.DependencyInjection;
using TIKSN.Cake.Core.Services;
using Xunit;

namespace TIKSN.Cake.Core.Tests
{
    public class TrashFolderServicesTests
    {
        [Fact]
        public void ResolveDependency()
        {
            var serviceProvider = DependencyInjectionHelper.CreateServiceProvider();

            var trashFolderServices = serviceProvider.GetRequiredService<ITrashFolderServices>();

            trashFolderServices.Should().NotBeNull();

            trashFolderServices = serviceProvider.GetRequiredService<ITrashFolderServices>();

            trashFolderServices.Should().NotBeNull();

            var idGenerators = serviceProvider.GetRequiredService<IIdGenerator<long>>();

            idGenerators.Should().NotBeNull();
        }
    }
}