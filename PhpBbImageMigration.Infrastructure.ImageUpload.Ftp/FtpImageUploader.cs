using PhpBbImageMigration.Domain.ImagesHandling;
using PhpBbImageMigration.Infrastructure.ImageUpload.Ftp.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PhpBbImageMigration.Infrastructure.ImageUpload.Ftp
{
    public class FtpImageUploader : IImageUploader
    {
        private bool disposedValue;

        private readonly HttpClient _client;
        private readonly FtpConfiguration _config;

        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

        public FtpImageUploader(FtpConfiguration ftpConfig, HttpClient client)
        {
            _client = client;
            _config = ftpConfig;
        }

        public async Task<bool> SaveAndUpload(string imageUrl)
        {
            string imgFile = Path.GetFileName(imageUrl);
            string ftpImagePath = new Uri(new Uri(_config.Host), imgFile).AbsoluteUri;

            if (await FileExists(ftpImagePath))
                return true;


            byte[] img = null;

            try
            {
                img = await _client.GetByteArrayAsync(imageUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting original image: {ex}");
            }

            if (img == null)
                return false;

            await _lock.WaitAsync();
            try
            {
                FtpWebRequest request = CreateFtpRequest(ftpImagePath, WebRequestMethods.Ftp.UploadFile);

                // Copy the contents of the file to the request stream.

                request.ContentLength = img.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(img, 0, img.Length);
                }

                using (FtpWebResponse response = (FtpWebResponse)(await request.GetResponseAsync()))
                {
                    return response.StatusCode == FtpStatusCode.ClosingData;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading file to ftp: {ex}");
                return false;
            }
            finally
            {
                _lock.Release();
            }
        }

        private async Task<bool> FileExists(string imageUrl)
        {
            FtpWebRequest request = CreateFtpRequest(imageUrl, WebRequestMethods.Ftp.GetFileSize);

            try
            {
                using var response = await request.GetResponseAsync();

                return true;
            }
            catch (WebException wex)
            {
                FtpWebResponse response = (FtpWebResponse)wex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    return false;
                }

                throw;
            }
        }

        private FtpWebRequest CreateFtpRequest(string path, string method)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(path);
            request.Method = method;
            request.Credentials = new NetworkCredential(_config.Username, _config.Password);

            return request;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _client.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~FtpImageUploader()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
