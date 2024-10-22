using Homee.BusinessLayer.Helpers;
using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using Homee.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Homee.Repositories.Repositories
{
    public class ContractRepository : BaseRepository<Contract>, IContractRepository
    {
        private readonly HomeedbContext _context;

        public ContractRepository() { }
        public ContractRepository(HomeedbContext context)
        {
            _context = context;
        }
        public bool CanCreate(ContractRequest request)
        {
            var oldContracts = _context.Contracts.Where(c => request.RoomId == c.RoomId);
            foreach (var oldContract in oldContracts)
            {
                var expiredDate = oldContract.CreateAt.Value.AddDays((double)oldContract.Duration);
                if (expiredDate <= DateTime.Now)
                {
                    return false;
                }
            }
            return true;
        }

        public Contract GetContract(int id)
        {
            try
            {
                return GetContracts().FirstOrDefault(c => c.ContractId == id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Contract> GetContractByCurrentUser(ClaimsPrincipal user)
        {
            try
            {
                var claim =  user.FindFirst(ClaimTypes.NameIdentifier);
                if (claim.Value == null || !int.TryParse(claim.Value, out int uId))
                {
                    return null;
                }

                return _context.Contracts
                .Include(c => c.Room).ThenInclude(c => c.Place).ThenInclude(c => c.Owner)
                .Include(c => c.Renter)
                    .Where(c => c.Room.Place.OwnerId == uId).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Contract> GetContracts()
        {
            return _context.Contracts
                .Include(c => c.Room).ThenInclude(c => c.Place).ThenInclude(c => c.Owner)
                .Include(c => c.Renter)
                .ToList();
        }
    }
}
