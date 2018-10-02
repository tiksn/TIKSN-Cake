using System;
using TIKSN.Time;

namespace TIKSN.Cake.Core.Services.VersioningStrategies
{
    public class NextMajorVersioningStrategy : IVersioningStrategy
    {
        private readonly ITimeProvider _timeProvider;

        public NextMajorVersioningStrategy(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
        }

        public Versioning.Version GetNextVersion(Versioning.Version latestVersion)
        {
            var nextRelease = new Version(
                latestVersion.Release.Major + 1,
                0,
                0,
                0);

            return new Versioning.Version(nextRelease, _timeProvider.GetCurrentTime());
        }
    }
}