using Unity;
using System.Configuration;

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
                RootPath = ConfigurationManager.AppSettings["rootPath"]
            });

            return container;
        }
    }
}
