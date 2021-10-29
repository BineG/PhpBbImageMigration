using Microsoft.Extensions.DependencyInjection;
using PhpBbImageMigration.Domain.App;
using PhpBbImageMigration.Domain.DataEntities;
using PhpBbImageMigration.Domain.ImagesHandling;
using PhpBbImageMigration.Domain.Posts;
using System;
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

        private readonly IServiceProvider _serviceProvider;
        private readonly IImageUploader _imageUploader;
        private readonly AppConfiguration _appConfiguration;

        public ImageMigrationWorker(IServiceProvider serviceProvider, IImageUploader imageUploader, AppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
            _serviceProvider = serviceProvider;
            _imageUploader = imageUploader;
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
                var postsRepository = _serviceProvider.GetRequiredService<IPostsRepository>();
                {
                    posts = await postsRepository.GetPosts(patterns, Take, page + Take);

                    var transformedPosts = posts
                        //.AsParallel()
                        .Select(async item =>
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
                            // save image and generate new path
                            string file = Path.GetFileName(match);
                                string httpImgUrl = new Uri(new Uri(_appConfiguration?.ImageUpload?.HttpRequestBasePath), file).AbsoluteUri;

                                if (await _imageUploader.SaveAndUpload(match))
                                {
                                    item.PostText = item.PostText.Replace(match, httpImgUrl);
                                }
                                await Task.Delay(200);
                            }

                            return item;
                        })
                        .ToList();

                    page++;

                    //await postsRepository.SaveChanges();
                }
            } while (posts.Any());
        }
    }
}
