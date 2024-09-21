using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.Repositories.IRepositories
{
    public interface IPostRepository : IBaseRepository<Post>
    {
        Task<int> DeletePost(int id);
        Task<IList<Post>> GetPosts();
        Task<Post> GetPostById(int id);
        Task<int> InsertPlacePost(PlacePostRequest model);
        Task<bool> CanInsert(PlacePostRequest model);
    }
}
