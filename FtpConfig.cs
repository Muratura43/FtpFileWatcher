using System;

namespace FtpFileWatcher
{
    public class FtpConfig
    {
        public string Host { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string RootPath { get; set; }
        public string WatchPath { get; set; }
    }

    public class FtpFile : IEquatable<FtpFile>
    {
        public string FullPath { get; set; }
        public long FileSize { get; set; }

        public bool Equals(FtpFile other)
        {
            return other != null &&
                FullPath == other.FullPath && 
                FileSize == other.FileSize;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as FtpFile);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(FullPath, FileSize).GetHashCode();
        }
    }
}
