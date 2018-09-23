using Microsoft.Extensions.Logging;

namespace TIKSN.Cake.Core.Services
{
    public interface ILocalizationKeysGenerator
    {
        void GenerateLocalizationKeys(ILogger logger, string @namespace, string @class, string outputFolder, string[] resxFiles);
    }
}