using Microsoft.Extensions.DependencyInjection;
using PhpBbImageMigration.Domain.App;
using PhpBbImageMigration.Domain.DataEntities;
using PhpBbImageMigration.Domain.ImagesHandling;
using PhpBbImageMigration.Domain.Posts;
using PhpBbImageMigration.Infrastructure.MySql;
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
            try
            {
                if (patterns == null || !patterns.Any())
                {
                    return;
                }

                var regexQuotes = new Regex("\"[^\"]*\"");
                var regexBrackets = new Regex("\\]\\S+\\[");

                List<Phpbb3Post> posts = null;

                int page = 0;
                int? lastPost = null;
                do
                {
                    var postsRepository = _serviceProvider.GetRequiredService<IPostsRepository>();
                    {
                        posts = await postsRepository.GetPosts(patterns, Take, lastPost);
                        
                        if (!posts.Any())
                            break;

                        List<Task<Phpbb3Post>> transformedPosts = posts
                            //.AsParallel()
                            .Select(async item =>
                            {
                                var matches = regexQuotes.Matches(item.PostText)
                                    // trim quotes
                                    .Select(x => x.Value.Trim('\"'))
                                    .Where(x =>
                                    {
                                        string extension = Path.GetExtension(x);

                                        return patterns.Any(p => x.Contains(p)) && (extension == ".jpg" || extension == ".jpeg" || extension == ".png");
                                    })
                                    .ToList();

                                matches.AddRange(
                                    regexBrackets.Matches(item.PostText)
                                        .Select(x => System.Web.HttpUtility.HtmlDecode(x.Value.Trim(']', '[')))
                                        .Where(x =>
                                        {
                                            string extension = Path.GetExtension(x);

                                            return patterns.Any(p => x.Contains(p)) && (extension == ".jpg" || extension == ".jpeg" || extension == ".png");
                                        })
                                );

                                foreach (string match in matches)
                                {
                                    // save image and generate new path
                                    string file = Path.GetFileName(match);
                                    string httpImgUrl = new Uri(new Uri(_appConfiguration?.ImageUpload?.HttpRequestBasePath), file).AbsoluteUri;

                                    if (await _imageUploader.SaveAndUpload(match))
                                    {
                                        item.PostText = item.PostText.Replace(match, httpImgUrl);
                                        item.PostText = item.PostText.Replace(EncodeDots(match), EncodeDots(httpImgUrl));
                                    }
                                    await Task.Delay(200);
                                }

                                return item;
                            })
                            .ToList();

                        await Task.WhenAll(transformedPosts);

                        lastPost = transformedPosts.LastOrDefault()?.Result.PostId;

                        await postsRepository.SaveChanges();

                        page++;
                    }
                } while (posts.Any());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error {ex}");
                throw;
            }
        }

        private string EncodeDots(string url)
        {
            return url.Replace(".", "&#46;").Replace(":", "&#58;");
        }
    }
}
