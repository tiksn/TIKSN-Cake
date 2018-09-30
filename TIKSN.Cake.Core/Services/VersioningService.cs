using Microsoft.Extensions.Logging;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.Linq;
using TIKSN.Cake.Core.Services.VersioningStrategies;

namespace TIKSN.Cake.Core.Services
{
    public class VersioningService
    {
        private readonly object _versionGetterLocker = new object();
        private readonly Dictionary<string, IVersioningStrategy> _versioningStrategies = new Dictionary<string, IVersioningStrategy>(StringComparer.OrdinalIgnoreCase);
        private readonly object _versionSetterLocker = new object();

        private Versioning.Version _latestVersion;
        private Versioning.Version _nextVersion;

        public VersioningService()
        {
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
                    _nextVersion = _versioningStrategies[nextVersionStrategyArgument].GetNextVersion(_latestVersion);

                logger.LogDebug($"Estimated next version to be '{_nextVersion}'.");
            }

            return _nextVersion;
        }

        public void SetVersions(ILogger logger, IEnumerable<NuGetVersion> versions)
        {
            SetVersions(logger, versions, v => (Versioning.Version)v);
        }

        public void SetVersions(ILogger logger, IEnumerable<SemanticVersion> versions)
        {
            SetVersions(logger, versions, v => (Versioning.Version)v);
        }

        public void SetVersions(ILogger logger, IEnumerable<Versioning.Version> versions)
        {
            SetVersions(logger, versions, v => v);
        }

        public void SetVersions(ILogger logger, IEnumerable<Version> versions)
        {
            SetVersions(logger, versions, v => new Versioning.Version(v));
        }

        private void SetVersions<T>(ILogger logger, IEnumerable<T> versions, Func<T, Versioning.Version> convert)
        {
            if (!versions.Any())
                throw new ArgumentException("Version list provided is empty");

            var latestVersion = versions.Max();

            lock (_versionSetterLocker)
            {
                if (_latestVersion != null)
                    throw new InvalidOperationException("Version already set. This operation can be done only once.");

                _latestVersion = convert(latestVersion);
            }
        }
    }
}