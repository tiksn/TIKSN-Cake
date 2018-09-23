namespace TIKSN.Cake.Core.Services
{
    public interface ILocalizationKeysGenerator
    {
        void GenerateLocalizationKeys(string @namespace, string @class, string outputFolder, string[] resxFiles);
    }
}