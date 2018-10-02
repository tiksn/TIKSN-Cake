using System;
using TIKSN.Time;

namespace TIKSN.Cake.Core.Services.VersioningStrategies
{
    public class NextMinorVersioningStrategy : IVersioningStrategy
    {
        private readonly ITimeProvider _timeProvider;

        public NextMinorVersioningStrategy(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider ?? throw new System.ArgumentNullException(nameof(timeProvider));
        }

        public Versioning.Version GetNextVersion(Versioning.Version latestVersion)
        {
            var nextRelease = new Version(
                latestVersion.Release.Major,
                latestVersion.Release.Minor == -1 ? 1 : latestVersion.Release.Minor + 1,
                0,
                0);

            return new Versioning.Version(nextRelease, _timeProvider.GetCurrentTime());
        }
    }
}