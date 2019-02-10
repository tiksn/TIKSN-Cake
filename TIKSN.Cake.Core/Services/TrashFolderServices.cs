using IdGen;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace TIKSN.Cake.Core.Services
{
    public class TrashFolderServices : ITrashFolderServices
    {
        private static object trashDirectoryFullPathLocker = new object();
        private readonly IIdGenerator<long> _idGenerator;
        private string _trashDirectoryFullPath;

        public TrashFolderServices(IIdGenerator<long> idGenerator)
        {
            _idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
        }

        public string CreateTrashSubFolder(ILogger logger, string subfolderName)
        {
            var subfolderFullPath = Path.Combine(_trashDirectoryFullPath, subfolderName);

            Directory.CreateDirectory(subfolderFullPath);

            logger.LogDebug($"Trash sub-folder is created in location '{subfolderFullPath}'.");

            return subfolderFullPath;
        }

        public string GetTrashFolder(ILogger logger)
        {
            if (_trashDirectoryFullPath == null)
                throw new SystemException($"Variable with name '{nameof(_trashDirectoryFullPath)}' is not set yet.");

            return _trashDirectoryFullPath;
        }

        public void SetTrashParentFolder(ILogger logger, string rootDirectoryFullPath)
        {
            if (rootDirectoryFullPath == null)
                throw new ArgumentNullException(nameof(rootDirectoryFullPath));

            if (!Directory.Exists(rootDirectoryFullPath))
                throw new ArgumentException($"Directory '{rootDirectoryFullPath}' does not exists.", nameof(rootDirectoryFullPath));

            rootDirectoryFullPath = Path.GetFullPath(rootDirectoryFullPath);

            lock (trashDirectoryFullPathLocker)
            {
                if (_trashDirectoryFullPath != null)
                    throw new SystemException($"Variable with name '{nameof(_trashDirectoryFullPath)}' is already set.");

                _trashDirectoryFullPath = Path.Combine(rootDirectoryFullPath, ".trash", _idGenerator.CreateId().ToString());
                Directory.CreateDirectory(_trashDirectoryFullPath);
                logger.LogDebug($"Trash folder is created in location '{_trashDirectoryFullPath}'.");
            }
        }
    }
}