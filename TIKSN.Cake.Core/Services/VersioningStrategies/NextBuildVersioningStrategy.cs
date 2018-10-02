using System;
using TIKSN.Time;


namespace TIKSN.Cake.Core.Services.VersioningStrategies
{
    public class NextBuildVersioningStrategy : IVersioningStrategy
    {
        private readonly ITimeProvider _timeProvider;

        public NextBuildVersioningStrategy(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
        }

        public Versioning.Version GetNextVersion(Versioning.Version latestVersion)
        {
            var nextRelease = new Version(
                latestVersion.Release.Major,
                latestVersion.Release.Minor,
                latestVersion.Release.Build == -1 ? 1 : latestVersion.Release.Build + 1,
                0);

            return new Versioning.Version(nextRelease, _timeProvider.GetCurrentTime());
        }
    }
}