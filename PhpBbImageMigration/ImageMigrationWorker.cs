using PhpBbImageMigration.Domain.DataEntities;
using PhpBbImageMigration.Domain.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhpBbImageMigration
{
    public class ImageMigrationWorker
    {
        private readonly IPostsRepository _postsRepository;

        public ImageMigrationWorker(IPostsRepository postsRepository)
        {
            _postsRepository = postsRepository;
        }

        public async Task Start()
        {
            List<Phpbb3Post> posts = await _postsRepository.GetPosts();
        }
    }
}
