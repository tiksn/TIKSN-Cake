using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NuGet.Versioning;
using TIKSN.Versioning;

namespace TIKSN.Cake.Core.Services
{
    public interface IVersioningService
    {
        Versioning.Version GetNextVersion(ILogger logger, string nextVersionArgument, string nextVersionStrategyArgument);
        void SetVersions(ILogger logger, IEnumerable<NuGetVersion> versions);
        void SetVersions(ILogger logger, IEnumerable<SemanticVersion> versions);
        void SetVersions(ILogger logger, IEnumerable<Versioning.Version> versions);
        void SetVersions(ILogger logger, IEnumerable<System.Version> versions);
    }
}