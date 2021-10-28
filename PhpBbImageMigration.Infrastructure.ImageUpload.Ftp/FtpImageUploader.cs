using PhpBbImageMigration.Domain.ImagesHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PhpBbImageMigration.Infrastructure.ImageUpload.Ftp
{
    public class FtpImageUploader : IImageUploader
    {
        private bool disposedValue;

        private readonly HttpClient _client;

        public FtpImageUploader(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> SaveAndUpload(string url)
        {
            byte[] img = await _client.GetByteArrayAsync(url);

            return null;
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
