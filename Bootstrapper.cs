using Unity;
using System.Configuration;
using FtpFileWatcher.Interfaces;
using FtpFileWatcher.Common;
using FtpFileWatcher.Business;

namespace FtpFileWatcher
{
    internal static class Bootstrapper
    {
        public static IUnityContainer Setup(this IUnityContainer container)
        {
            container.RegisterFactory<FtpConfig>((c) => new FtpConfig()
            {
                Host = ConfigurationManager.AppSettings["host"],
                UserId = ConfigurationManager.AppSettings["userId"],
                Password = ConfigurationManager.AppSettings["password"],
                RootPath = ConfigurationManager.AppSettings["rootPath"],
                WatchPath = ConfigurationManager.AppSettings["watchPath"]
            });

            container.RegisterType<IFtpHandler, FtpHandler>();
            container.RegisterType<IFileWatcher, FileWatcher>();

            return container;
        }
    }
}
