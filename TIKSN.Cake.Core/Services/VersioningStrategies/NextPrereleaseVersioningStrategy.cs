using System;
using TIKSN.Time;

namespace TIKSN.Cake.Core.Services.VersioningStrategies
{
    public class NextPrereleaseVersioningStrategy : IVersioningStrategy
    {
        private readonly ITimeProvider _timeProvider;

        public NextPrereleaseVersioningStrategy(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
        }

        public Versioning.Version GetNextVersion(Versioning.Version latestVersion)
        {
            if (latestVersion.Stability == Versioning.Stability.Stable)
                throw new ArgumentOutOfRangeException("Cannot estimate next pre-release version for stable latest version.");

            return new Versioning.Version(latestVersion.Release, latestVersion.Milestone, latestVersion.PrereleaseNumber + 1, _timeProvider.GetCurrentTime());
        }
    }
}