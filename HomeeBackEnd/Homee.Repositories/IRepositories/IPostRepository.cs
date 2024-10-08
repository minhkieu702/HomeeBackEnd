using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Homee.Repositories.IRepositories
{
    public interface IPostRepository : IBaseRepository<Post>
    {
        Task<int> DeletePost(int id);
        Task<IList<Post>> GetPosts();
        Task<Post> GetPostById(int id);
        Task<int> InsertPlacePost(PlacePostRequest model, ClaimsPrincipal user);
        Task<bool> CanInsert(PlacePostRequest model);
        Task<int> UpdatePost(int id, PostRequest model);
        Task<int> UpdatePlacePost(int id, PlacePostRequest model);
        Task<IEnumerable<object>> GetPosts(ClaimsPrincipal user);
    }
}
