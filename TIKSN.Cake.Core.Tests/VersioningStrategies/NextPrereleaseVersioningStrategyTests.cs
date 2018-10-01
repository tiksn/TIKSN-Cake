using FluentAssertions;
using NSubstitute;
using NuGet.Versioning;
using TIKSN.Cake.Core.Services.VersioningStrategies;
using TIKSN.Time;
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
        [InlineData("1.2.3.4-alpha.1", "1.2.3.4-alpha.2")]
        [InlineData("1.2.3.4-beta.14", "1.2.3.4-beta.15")]
        [InlineData("1.2.3.4-rc.14", "1.2.3.4-rc.15")]
        public void GetNextVersionTest(string latestVersionString, string expectedNextVersionString)
        {
            var latestVersion = (Version)NuGetVersion.Parse(latestVersionString);
            var expectedNextVersion = (Version)NuGetVersion.Parse(expectedNextVersionString);
            var timeProvider = Substitute.For<ITimeProvider>();
            var strategy = new NextPrereleaseVersioningStrategy(timeProvider);

            var nextVersion = strategy.GetNextVersion(latestVersion);

            nextVersion.Should().Be(expectedNextVersion);
        }

        [Theory]
        [InlineData("1.2.0")]
        [InlineData("1.2.3")]
        [InlineData("1.2.3.4")]
        public void GetNextVersionThrowsException(string latestVersionString)
        {
            var latestVersion = (Version)NuGetVersion.Parse(latestVersionString);
            var timeProvider = Substitute.For<ITimeProvider>();
            var strategy = new NextPrereleaseVersioningStrategy(timeProvider);

            Assert.Throws<System.ArgumentOutOfRangeException>(() => strategy.GetNextVersion(latestVersion));
        }
    }
}