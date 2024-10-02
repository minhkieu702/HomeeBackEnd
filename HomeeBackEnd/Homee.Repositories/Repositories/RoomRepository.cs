using Homee.BusinessLayer.Helpers;
using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using Homee.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Homee.Repositories.Repositories
{
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        private readonly HomeedbContext _context;

        public RoomRepository(HomeedbContext context)
        {
            _context = context;
        }
        public async Task<List<Room>> GetRoomByCurrentUser(ClaimsPrincipal user)
        {
            try
            {
                var claim = user.FindFirst(ClaimTypes.NameIdentifier);
                if (claim.Value == null || !int.TryParse(claim.Value, out int uId))
                {
                    return null;
                }

                return await _context.Rooms.IncludeAll().Where(c => c.Place.OwnerId == uId).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Room>> GetRoomByPlace(int placeId)
        {
            return await _context.Rooms.IncludeAll().Where(c => c.PlaceId == placeId).ToListAsync();
        }

        public async Task<int> UpdateRoom(int id, Room room)
        {
            try
            {
                var result = await _context.Rooms.FindAsync(id);
                if (result != null) return 0;
                result = room;
                result.RoomId = id;

                _context.Rooms.Update(result);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
