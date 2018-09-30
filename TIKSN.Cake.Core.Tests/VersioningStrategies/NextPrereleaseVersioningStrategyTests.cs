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
        public void GetNextVersionTest(string latestVersionString, string expectedNextVersionString)
        {
            var latestVersion = (Version)NuGetVersion.Parse(latestVersionString);
            var expectedNextVersion = (Version)NuGetVersion.Parse(expectedNextVersionString);

            var strategy = new NextPrereleaseVersioningStrategy();

            var nextVersion = strategy.GetNextVersion(latestVersion);

            nextVersion.Should().Be(expectedNextVersion);
        }
    }
}