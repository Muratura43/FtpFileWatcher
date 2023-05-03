using System;
using System.IO;

namespace FtpFileWatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            var handler = new FtpHandler();
            var files = handler.GetDirectoryListing();

            foreach (var f in files)
            {
                Console.WriteLine(f + " : " + handler.GetFileSize(Path.GetFileName(f)) + " bytes");
            }
        }
    }
}
