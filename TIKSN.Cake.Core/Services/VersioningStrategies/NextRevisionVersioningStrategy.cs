using System;
using TIKSN.Time;

namespace TIKSN.Cake.Core.Services.VersioningStrategies
{
    public class NextRevisionVersioningStrategy : IVersioningStrategy
    {
        private readonly ITimeProvider _timeProvider;

        public NextRevisionVersioningStrategy(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
        }

        public Versioning.Version GetNextVersion(Versioning.Version latestVersion)
        {
            var nextRelease = new Version(
                latestVersion.Release.Major,
                latestVersion.Release.Minor,
                latestVersion.Release.Build == -1 ? 0 : latestVersion.Release.Build,
                latestVersion.Release.Revision == -1 || latestVersion.Release.Revision == 0 ? 1 : latestVersion.Release.Revision + 1);

            return new Versioning.Version(nextRelease, _timeProvider.GetCurrentTime());
        }
    }
}