using Homee.BusinessLayer.Commons;
using Homee.DataLayer.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Homee.BusinessLayer.IServices
{
    public interface IRoomService
    {
        Task<IHomeeResult> GetRooms();
        Task<IHomeeResult> GetRoomByPlace(int placeId);
        Task<IHomeeResult> Update(int roomId, RoomRequest room);
        Task<IHomeeResult> Create(RoomRequest room);
        Task<IHomeeResult> Block(int roomId);
        Task<IHomeeResult> GetRoom(int roomId);
        Task<IHomeeResult> GetRoomByCurrentUser(ClaimsPrincipal user);
    }
}
