using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhpBbImageMigration.Domain.ImagesHandling
{
    public interface IImageUploader : IDisposable
    {
        Task<bool> SaveAndUpload(string url);
    }
}
