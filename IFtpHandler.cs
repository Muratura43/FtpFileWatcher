using System.Collections.Generic;

namespace FtpFileWatcher
{
    public interface IFtpHandler
    {
        IEnumerable<string> GetDirectoryListing();
        long GetFileSize(string filename);
        void UploadFile(string fullPath);
    }
}
