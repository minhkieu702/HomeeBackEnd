using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using Homee.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.Repositories.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        private readonly HomeedbContext _context;

        public PostRepository(){}

        public PostRepository(HomeedbContext context)
        {
            _context = context;
        }

        public Task<bool> CanInsert(PlacePostRequest model)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeletePost(int id)
        {
            var check = 0;
            using (var p = _context.Database.BeginTransaction())
            {
                try
                {
                    var imgs = _context.Images.Where(c => c.PostId == id);
                    if (imgs != null  && imgs.Count() > 0)
                    {
                        _context.Images.RemoveRange(imgs);
                        check = await _context.SaveChangesAsync();
                        if (check <= 0)
                        {
                            await p.RollbackAsync();
                            return check;
                        }
                    }

                    var fps = _context.FavoritePosts.Where(c => c.PostId == id);
                    if (fps != null && fps.Count() > 0)
                    {
                        _context.FavoritePosts.RemoveRange();
                        check = await _context.SaveChangesAsync();
                        if (check <= 0)
                        {
                            await p.RollbackAsync();
                            return check;
                        }
                    }

                    var post = _context.Posts.FirstOrDefault(c => c.PostId == id);
                    if (post != null)
                    {
                        _context.Posts.Remove(post);
                        check = await _context.SaveChangesAsync();
                        if (check <= 0)
                        {
                            await p.RollbackAsync();
                            return check;
                        }
                    }
                    await p.CommitAsync();
                    return check;
                }
                catch (Exception)
                {
                    await p.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<Post> GetPostById(int id)
        {
            var post = await _context.Posts.Include(c => c.Images)
                .Include(c => c.Place).ThenInclude(c => c.Owner)
                .Include(c => c.Place).ThenInclude(c => c.CategoryPlaces)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.PostId == id);
            return post;
        }

        public async Task<IList<Post>> GetPosts()
        {
            var posts = await _context.Posts.Include(c => c.Images)
                .Include(c => c.Place).ThenInclude(c => c.Owner)
                .Include(c => c.Place).ThenInclude(c => c.CategoryPlaces).ThenInclude(c => c.Category)
                .ToListAsync();
            return posts;
        }

        public Task<int> InsertPlacePost(PlacePostRequest model)
        {
            throw new NotImplementedException();
        }
    }
}
