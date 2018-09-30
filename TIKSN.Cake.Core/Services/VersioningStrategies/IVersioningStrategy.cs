using TIKSN.Versioning;

namespace TIKSN.Cake.Core.Services.VersioningStrategies
{
    public interface IVersioningStrategy
    {
        Version GetNextVersion(Version latestVersion);
    }
}