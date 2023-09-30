using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Unity;

namespace FtpFileWatcher
{
    public class FtpHandler : IFtpHandler
    {
        private FtpConfig _config;

        public FtpHandler(FtpConfig config)
        {
            _config = config;
        }

        public IEnumerable<string> GetDirectoryListing()
        {
            var request = GetRequest();
            if (request == null)
            {
                yield break;
            }

            request.Method = WebRequestMethods.Ftp.ListDirectory;

            using (var response = (FtpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != FtpStatusCode.OpeningData)
                {
                    yield break;
                }

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    while (!reader.EndOfStream)
                    {
                        var s = reader.ReadLine();

                        if (s.Length >= 1)
                        {
                            yield return s;
                        }
                    }
                }
            }
        }

        public long GetFileSize(string filename)
        {
            long result = 0;

            var request = GetRequest(filename);
            if (request == null)
            {
                return -1;
            }

            request.Method = WebRequestMethods.Ftp.GetFileSize;

            try
            {
                using (var response = (FtpWebResponse)request.GetResponse())
                {
                    result = response.ContentLength;
                }
            }
            catch (Exception)
            {
                result = -1;
            }

            return result;
        }

        public void UploadFile(string fullPath)
        {
            //TODO: add async version

            if (!File.Exists(fullPath))
            {
                return;
            }

            var request = GetRequest(Path.GetFileName(fullPath));
            if (request == null)
            {
                return;
            }

            request.Method = WebRequestMethods.Ftp.UploadFile;

            byte[] fileContents;
            using (FileStream fs = File.OpenRead(fullPath))
            {
                fileContents = new byte[fs.Length];
                fs.Read(fileContents, 0, fileContents.Length);
            }

            request.KeepAlive = true;
            request.ContentLength = fileContents.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(fileContents, 0, fileContents.Length);
            }

            var response = (FtpWebResponse)request.GetResponse();

            Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);

            response.Close();
        }

        public string ListDirectoryDetails()
        {
            try
            {
                var request = GetRequest();
                if (request == null)
                {
                    return null;
                }

                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                using (var response = (FtpWebResponse)request.GetResponse())
                {
                    Stream responseStream = response.GetResponseStream();
                    var reader = new StreamReader(responseStream);

                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        private FtpWebRequest GetRequest(string fileName = null)
        {
            try
            {
                var path = fileName == null
                    ? _config.Host + _config.RootPath
                    : Path.Combine(_config.Host + _config.RootPath, fileName);

                var request = (FtpWebRequest)WebRequest.Create(path);
                request.Credentials = new NetworkCredential(_config.UserId, _config.Password);

                return request;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
