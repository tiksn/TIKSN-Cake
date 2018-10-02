using Microsoft.Extensions.Logging;
using NuGet.Versioning;
using System.Collections.Generic;

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