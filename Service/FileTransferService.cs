using FtpFileWatcher.Interfaces;
using Topshelf;
using Unity;

namespace FtpFileWatcher.Service
{
    public class FileTransferService
    {
        private readonly IUnityContainer _container;
        private IFileWatcher _fileWatcher;

        public FileTransferService(IUnityContainer container)
        {
            _container = container;
        }

        public void Start()
        {
            _fileWatcher = _container.Resolve<IFileWatcher>();
        }

        public void Stop()
        {
            _fileWatcher.Dispose();
        }
    }

    internal static class ConfigureService
    {
        internal static void Configure(IUnityContainer container)
        {
            HostFactory.Run(configure =>
            {
                configure.Service<FileTransferService>(service =>
                {
                    service.ConstructUsing(s => new FileTransferService(container));
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
                });

                configure.RunAsLocalSystem();
                configure.SetServiceName("MyFileTransferToFtpService");
                configure.SetDisplayName("My File Transfer to FTP Service");
                configure.SetDescription("My File Transfer to FTP Service");
            });
        }
    }
}
