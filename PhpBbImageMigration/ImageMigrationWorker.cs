using PhpBbImageMigration.Domain.DataEntities;
using PhpBbImageMigration.Domain.Posts;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PhpBbImageMigration
{
    public class ImageMigrationWorker
    {
        private const int Take = 200;

        private readonly IPostsRepository _postsRepository;

        public ImageMigrationWorker(IPostsRepository postsRepository)
        {
            _postsRepository = postsRepository;
        }

        public async Task Start(string[] patterns)
        {
            if (patterns == null || !patterns.Any())
            {
                return;
            }

            var regex = new Regex("\"[^\"]*\"");

            List<Phpbb3Post> posts = null;

            int page = 0;
            do
            {
                posts = await _postsRepository.GetPosts(patterns, Take, page + Take);

                var transformedPosts = posts
                    .AsParallel()
                    .Select(item =>
                    {
                        var matches = regex.Matches(item.PostText)
                        // trim quotes
                            .Select(x => x.Value.Trim('\"'))
                            .Where(x =>
                            {
                                string extension = Path.GetExtension(x);

                                return patterns.Any(p => x.Contains(p)) && (extension == ".jpg" || extension == ".jpeg" || extension == ".png");
                            })
                            .ToList();

                        foreach (string match in matches)
                        {
                            // TODO save image and generate new path
                            //item.PostText = item.PostText.Replace()
                        }

                        return item;
                    })
                    .ToList();

                page++;
            } while (posts.Any());

            await _postsRepository.SaveChanges();
        }
    }
}
