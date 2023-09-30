using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Unity;

namespace FtpFileWatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new UnityContainer();
            container.Setup();

            var handler = new FtpHandler(container.Resolve<FtpConfig>());
            var files = handler.GetDirectoryListing();

            foreach (var f in files)
            {
                Console.WriteLine(f + " : " + handler.GetFileSize(Path.GetFileName(f)) + " bytes");
            }

            var s = new ManualResetEventSlim();

            var l = new object();
            var t = new Task(() =>
            {
                lock (l)
                {
                    var fw = new FileWatcher(container.Resolve<FtpConfig>());
                }
            });

            t.Start();

            s.Wait();
        }
    }
}
