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
            throw new System.NotImplementedException();
        }
    }
}