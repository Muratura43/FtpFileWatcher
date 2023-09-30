using FtpFileWatcher.Interfaces;
using FtpFileWatcher.Service;
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

            ConfigureService.Configure(container);
        }
    }
}
