using System;
using System.IO;
using Unity;

namespace FtpFileWatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new UnityContainer();

            var handler = new FtpHandler(container);
            var files = handler.GetDirectoryListing();

            foreach (var f in files)
            {
                Console.WriteLine(f + " : " + handler.GetFileSize(Path.GetFileName(f)) + " bytes");
            }
        }
    }
}
