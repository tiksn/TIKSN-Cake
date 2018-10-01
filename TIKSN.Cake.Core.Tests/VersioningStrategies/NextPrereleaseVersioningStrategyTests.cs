using FluentAssertions;
using NuGet.Versioning;
using TIKSN.Cake.Core.Services.VersioningStrategies;
using TIKSN.Versioning;
using Xunit;

namespace TIKSN.Cake.Core.Tests.VersioningStrategies
{
    public class NextPrereleaseVersioningStrategyTests
    {
        [Theory]
        [InlineData("1.2.3-alpha.1", "1.2.3-alpha.2")]
        [InlineData("1.2.3-beta.14", "1.2.3-beta.15")]
        [InlineData("1.2.3-rc.14", "1.2.3-rc.15")]
        [InlineData("1.2.3", "1.2.3-alpha.1")]
        public void GetNextVersionTest(string latestVersionString, string expectedNextVersionString)
        {
            var latestVersion = (Version)NuGetVersion.Parse(latestVersionString);
            var expectedNextVersion = (Version)NuGetVersion.Parse(expectedNextVersionString);

            var strategy = new NextPrereleaseVersioningStrategy();

            var nextVersion = strategy.GetNextVersion(latestVersion);

            nextVersion.Should().Be(expectedNextVersion);
        }

        [Theory]
        [InlineData("1.2.3")]
        [InlineData("1.2.3.4")]
        public void GetNextVersionThrowsException(string latestVersionString)
        {
            var latestVersion = (Version)NuGetVersion.Parse(latestVersionString);

            var strategy = new NextPrereleaseVersioningStrategy();

            Assert.Throws<System.Exception>(() => strategy.GetNextVersion(latestVersion));
        }
    }
}