using Microsoft.Extensions.Logging;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using TIKSN.Cake.Core.Services.VersioningStrategies;

namespace TIKSN.Cake.Core.Services
{
    public class VersioningService
    {
        private readonly object _versionGetterLocker = new object();
        private readonly object _versionSetterLocker = new object();

        private Versioning.Version[] _versions;
        private Versioning.Version _nextVersion;

        private readonly Dictionary<string, IVersioningStrategy> _versioningStrategies = new Dictionary<string, IVersioningStrategy>(StringComparer.OrdinalIgnoreCase);

        public VersioningService()
        {

        }

        public void SetVersions(ILogger logger, IEnumerable<NuGetVersion> versions, bool ignoreUnconventionalVersions)
        {
            SetVersions(logger, versions, ignoreUnconventionalVersions, v => (Versioning.Version)v);
        }

        public void SetVersions(ILogger logger, IEnumerable<SemanticVersion> versions, bool ignoreUnconventionalVersions)
        {
            SetVersions(logger, versions, ignoreUnconventionalVersions, v => (Versioning.Version)v);
        }

        public void SetVersions(ILogger logger, IEnumerable<Versioning.Version> versions, bool ignoreUnconventionalVersions)
        {
            SetVersions(logger, versions, ignoreUnconventionalVersions, v => v);
        }

        public void SetVersions(ILogger logger, IEnumerable<Version> versions, bool ignoreUnconventionalVersions)
        {
            SetVersions(logger, versions, ignoreUnconventionalVersions, v => new Versioning.Version(v));
        }

        public Versioning.Version GetNextVersion(ILogger logger, string nextVersionArgument, string nextVersionStrategyArgument)
        {
            if (_nextVersion != null)
                return _nextVersion;

            lock (_versionGetterLocker)
            {
                if (_nextVersion != null)
                    return _nextVersion;

                if (nextVersionArgument != null && nextVersionStrategyArgument != null)
                    throw new ArgumentException("Next Version and Next Version Strategy arguments cannot be passed at once.");

                if (nextVersionArgument != null)
                    _nextVersion = (Versioning.Version)NuGetVersion.Parse(nextVersionArgument);
                else
                    _nextVersion = _versioningStrategies[nextVersionStrategyArgument].GetNextVersion(_versions);

                logger.LogDebug($"Estimated next version to be '{_nextVersion}'.");
            }

            return _nextVersion;
        }

        private void SetVersions<T>(ILogger logger, IEnumerable<T> versions, bool ignoreUnconventionalVersions, Func<T, Versioning.Version> convert)
        {
            var results = new List<Versioning.Version>();

            foreach (var version in versions)
            {
                try
                {
                    var result = convert(version);
                    results.Add(result);
                }
                catch (Exception ex)
                {
                    if (ignoreUnconventionalVersions)
                    {
                        logger.LogError(ex, ex.Message);

                        throw;
                    }

                    logger.LogWarning($"Version '{version}' cannot be converted to '{typeof(Versioning.Version).FullName}'.");
                }
            }

            lock (_versionSetterLocker)
            {
                if (_versions != null)
                    throw new InvalidOperationException("Version already set. This operation can be done only once.");

                _versions = results.ToArray();
            }
        }
    }
}