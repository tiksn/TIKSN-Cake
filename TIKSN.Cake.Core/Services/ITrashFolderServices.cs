﻿using Microsoft.Extensions.Logging;

namespace TIKSN.Cake.Core.Services
{
    public interface ITrashFolderServices
    {
        void SetTrashParentFolder(ILogger logger, string rootDirectoryFullPath);

        string CreateTrashSubFolder(ILogger logger, string subfolderName);
    }
}
