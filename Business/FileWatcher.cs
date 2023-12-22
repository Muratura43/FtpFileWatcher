using FtpFileWatcher.Common;
using FtpFileWatcher.Interfaces;
using System;
using System.IO;
using System.Runtime.Caching;

namespace FtpFileWatcher.Business
{
    public class FileWatcher : IFileWatcher
    {
        private readonly FileSystemWatcher _watcher;
        private readonly FtpConfig _config;
        private readonly IFtpHandler _ftpHandler;

        private readonly MemoryCache _cache = MemoryCache.Default;

        public FileWatcher(FtpConfig config, IFtpHandler ftpHandler)
        {
            _config = config;
            _ftpHandler = ftpHandler;

            _watcher = new FileSystemWatcher(_config.WatchPath);

            _watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;
            _watcher.Filter = "*.*";
            _watcher.IncludeSubdirectories = true;
            
            _watcher.Changed += OnChanged;
            _watcher.Created += OnCreated;
            _watcher.Deleted += OnDeleted;
            _watcher.Renamed += OnRenamed;
            _watcher.Error += OnError;

            _watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"Changed: {e.FullPath}");

            AddToCache(e.FullPath);
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"Created: {e.FullPath}");

            AddToCache(e.FullPath);
        }

        private void OnDeleted(object sender, FileSystemEventArgs e) =>
            Console.WriteLine($"Deleted: {e.FullPath}");

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"Renamed:");
            Console.WriteLine($"    Old: {e.OldFullPath}");
            Console.WriteLine($"    New: {e.FullPath}");
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine("ERROR: ");
            PrintException(e.GetException());
        }

        private void PrintException(Exception ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();

                PrintException(ex.InnerException);
            }
        }

        private void AddToCache(string fullPath)
        {
            var item = new CacheItem(fullPath);
            var policy = new CacheItemPolicy
            {
                RemovedCallback = (args) => 
                { 
                    if (args.RemovedReason == CacheEntryRemovedReason.Expired)
                    {
                        _ftpHandler.UploadFile(args.CacheItem.Key);
                    }
                    else
                    {
                        Console.WriteLine($"WARNING: {args.CacheItem.Key} was removed unexpectedly from cache and may not be processed. Reason: {args.RemovedReason}.");
                    }
                },
                SlidingExpiration = TimeSpan.FromSeconds(_config.CacheSeconds)
            };
        }

        public void Dispose()
        {
            _watcher.Dispose();
            _cache.Dispose();
        }
    }
}
