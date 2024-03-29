﻿using System.Collections.Generic;

namespace FtpFileWatcher.Interfaces
{
    public interface IFtpHandler
    {
        IEnumerable<string> GetDirectoryListing();
        long GetFileSize(string filename);
        void UploadFile(string fullPath);
    }
}
