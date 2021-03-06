using Microsoft.EntityFrameworkCore;
using PhpBbImageMigration.Domain.DataEntities;
using PhpBbImageMigration.Domain.Posts;
using PhpBbImageMigration.Infrastructure.MySql.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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
                    _context.Dispose();
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

        public async Task<List<Phpbb3Post>> GetPosts(string[] patterns, int take, int? lastPostId)
        {
            if (!patterns.Any())
                return new List<Phpbb3Post>();

            var query = _context.Phpbb3Posts
                .OrderBy(p => p.PostId)
                .Where(p => p.TopicId == 35038)
                .AsQueryable();

            //string pat = patterns.FirstOrDefault();
            //var patternQuery = QueryByPattern(query, patterns.FirstOrDefault());
            //patternQuery = patternQuery.Union(QueryByPattern(query, HttpUtility.HtmlEncode(patterns.FirstOrDefault())));
            IQueryable<Phpbb3Post> patternQuery = _context.Phpbb3Posts.Where(p => false);

            for (int i = 0; i < patterns.Length; i++)
            {
                string patt = patterns.ElementAt(i);
                patternQuery = patternQuery.Union(QueryByPattern(query, patt));
                patternQuery = patternQuery.Union(QueryByPattern(query, patt.Replace(".", "&#46;")));
            }

            return await patternQuery
                .Distinct()
                .Where(p => !lastPostId.HasValue || p.PostId > lastPostId)
                .Take(take)
                .ToListAsync();
        }

        private IQueryable<Phpbb3Post> QueryByPattern(IQueryable<Phpbb3Post> posts, string pattern)
        {
            return posts.Where(p => p.PostText.Contains(pattern));
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
