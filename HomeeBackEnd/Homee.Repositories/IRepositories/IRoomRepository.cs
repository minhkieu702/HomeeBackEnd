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
    public interface IRoomRepository : IBaseRepository<Room>
    {
        Task<List<Room>> GetRoomByCurrentUser(ClaimsPrincipal user);
        Task<List<Room>> GetRoomByPlace(int placeId);
        Task<int> UpdateRoom(int id, Room room);
    }
}
