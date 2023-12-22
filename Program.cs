using FtpFileWatcher.Service;
using Unity;

namespace FtpFileWatcher
{
    class Program
    {
        static void Main()
        {
            var container = new UnityContainer();
            container.Setup();

            ConfigureService.Configure(container);
        }
    }
}
