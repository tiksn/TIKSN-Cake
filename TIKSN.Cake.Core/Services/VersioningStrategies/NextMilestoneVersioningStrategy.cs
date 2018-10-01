using System;
using TIKSN.Time;

namespace TIKSN.Cake.Core.Services.VersioningStrategies
{
    public class NextMilestoneVersioningStrategy : IVersioningStrategy
    {
        private readonly ITimeProvider _timeProvider;

        public NextMilestoneVersioningStrategy(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
        }

        public Versioning.Version GetNextVersion(Versioning.Version latestVersion)
        {
            if (latestVersion.Stability == Versioning.Stability.Stable)
                throw new ArgumentOutOfRangeException("Cannot estimate next milestone version for stable latest version.");

            var nextMilestone = latestVersion.Milestone + 1;
            if (nextMilestone == Versioning.Milestone.Release)
                return new Versioning.Version(latestVersion.Release, nextMilestone, _timeProvider.GetCurrentTime());

            return new Versioning.Version(latestVersion.Release, nextMilestone, 1, _timeProvider.GetCurrentTime());
        }
    }
}