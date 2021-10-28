using PhpBbImageMigration.Domain.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhpBbImageMigration.Domain.Posts
{
    public interface IPostsRepository : IDisposable
    {
        Task<List<Phpbb3Post>> GetPosts(string[] patterns, int take, int skip);

        Task SaveChanges();
    }
}
