using Microsoft.Extensions.Logging;

namespace TIKSN.Cake.Core.Services
{
    public interface ITrashFolderServices
    {
        string CreateTrashSubFolder(ILogger logger, string subfolderName);

        string GetTrashFolder(ILogger logger);

        void SetTrashParentFolder(ILogger logger, string rootDirectoryFullPath);
    }
}