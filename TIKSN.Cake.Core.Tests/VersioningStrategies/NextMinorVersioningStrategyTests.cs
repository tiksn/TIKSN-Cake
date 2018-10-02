using FluentAssertions;
using NSubstitute;
using NuGet.Versioning;
using TIKSN.Cake.Core.Services.VersioningStrategies;
using TIKSN.Time;
using TIKSN.Versioning;
using Xunit;

namespace TIKSN.Cake.Core.Tests.VersioningStrategies
{
    public class NextMinorVersioningStrategyTests
    {
        [Theory]
        [InlineData("1.2.3-alpha.1", "1.3.0")]
        [InlineData("1.2.3-alpha.12", "1.3.0")]
        [InlineData("1.2.3-beta.14", "1.3.0")]
        [InlineData("1.2.3-rc.14", "1.3.0")]
        [InlineData("1.2.3.4-alpha.1", "1.3.0")]
        [InlineData("1.2.3.4-alpha.12", "1.3.0")]
        [InlineData("1.2.3.4-beta.14", "1.3.0")]
        [InlineData("1.2.3.4-rc.14", "1.3.0")]
        [InlineData("1.2.0", "1.3.0")]
        [InlineData("1.2", "1.3.0")]
        [InlineData("1.2.3", "1.3.0")]
        [InlineData("1.2.3.4", "1.3.0")]
        public void GetNextVersionTest(string latestVersionString, string expectedNextVersionString)
        {
            var latestVersion = (Version)NuGetVersion.Parse(latestVersionString);
            var expectedNextVersion = (Version)NuGetVersion.Parse(expectedNextVersionString);
            var timeProvider = Substitute.For<ITimeProvider>();
            var strategy = new NextMinorVersioningStrategy(timeProvider);

            var nextVersion = strategy.GetNextVersion(latestVersion);

            nextVersion.Should().Be(expectedNextVersion);
        }
    }
}