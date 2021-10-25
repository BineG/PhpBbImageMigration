using Microsoft.EntityFrameworkCore;
using PhpBbImageMigration.Domain.DataEntities;
using PhpBbImageMigration.Domain.Posts;
using PhpBbImageMigration.Infrastructure.MySql.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhpBbImageMigration.Infrastructure.MySql
{
    public class MysqlPostsRepository : IPostsRepository
    {
        private bool disposedValue;

        private readonly PhpbbContext _context;

        public MysqlPostsRepository(PhpbbContext context)
        {
            _context = context;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~MysqlPostsRepository()
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

        public async Task<List<Phpbb3Post>> GetPosts()
        {
            return await _context.Phpbb3Posts
                .Where(p => p.PostText.Contains("shrani.si"))
                .ToListAsync();
        }
    }
}
