﻿using Microsoft.Extensions.Logging;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.Linq;
using TIKSN.Cake.Core.Services.VersioningStrategies;
using TIKSN.Time;

namespace TIKSN.Cake.Core.Services
{
    public class VersioningService : IVersioningService
    {
        private readonly object _versionGetterLocker;
        private readonly Dictionary<string, IVersioningStrategy> _versioningStrategies = new Dictionary<string, IVersioningStrategy>(StringComparer.OrdinalIgnoreCase);
        private readonly object _versionSetterLocker;

        private Versioning.Version _latestVersion;
        private Versioning.Version _nextVersion;

        public VersioningService(ITimeProvider timeProvider)
        {
            _versionGetterLocker = new object();
            _versionSetterLocker = new object();

            _versioningStrategies.Add(string.Empty, new NextPrereleaseVersioningStrategy(timeProvider));
            _versioningStrategies.Add("prerelease", new NextPrereleaseVersioningStrategy(timeProvider));
            _versioningStrategies.Add("milestone", new NextMilestoneVersioningStrategy(timeProvider));
            _versioningStrategies.Add("revision", new NextRevisionVersioningStrategy(timeProvider));
            _versioningStrategies.Add("build", new NextBuildVersioningStrategy(timeProvider));
            _versioningStrategies.Add("minor", new NextMinorVersioningStrategy(timeProvider));
            _versioningStrategies.Add("major", new NextMajorVersioningStrategy(timeProvider));
        }

        public Versioning.Version GetNextVersion(ILogger logger, string nextVersionArgument, string nextVersionStrategyArgument)
        {
            if (!ReferenceEquals(_nextVersion, null))
                return _nextVersion;

            lock (_versionGetterLocker)
            {
                if (!ReferenceEquals(_nextVersion, null))
                    return _nextVersion;

                if (nextVersionArgument != null && nextVersionStrategyArgument != null)
                    throw new ArgumentException("Next Version and Next Version Strategy arguments cannot be passed at once.");

                if (nextVersionArgument != null)
                    _nextVersion = (Versioning.Version)NuGetVersion.Parse(nextVersionArgument);
                else
                    _nextVersion = _versioningStrategies[nextVersionStrategyArgument ?? string.Empty].GetNextVersion(_latestVersion);

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

            logger.LogDebug($"Maximum of passed versions is {latestVersion}");

            lock (_versionSetterLocker)
            {
                if (!ReferenceEquals(_latestVersion, null))
                    throw new InvalidOperationException("Version already set. This operation can be done only once.");

                _latestVersion = convert(latestVersion);

                logger.LogDebug($"Latest version is determined to be {_latestVersion}");
            }
        }
    }
}