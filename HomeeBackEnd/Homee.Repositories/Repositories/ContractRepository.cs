using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using Homee.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Homee.Repositories.Repositories
{
    public class ContractRepository : BaseRepository<Contract>, IContractRepository
    {
        private readonly HomeeDbContext _context;

        public ContractRepository() { }
        public ContractRepository(HomeeDbContext context)
        {
            _context = context;
        }
        public bool CanCreate(ContractRequest request)
        {
            var oldContracts = _context.Contracts.Where(c => request.PlaceId == c.PlaceId);
            foreach (var oldContract in oldContracts)
            {
                var expiredDate = oldContract.CreatedAt.AddDays((double)oldContract.Duration);
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
                var contract = _context.Contracts.Include(c => c.Render).Include(c => c.Place).ThenInclude(c => c.Owner).FirstOrDefault(c => c.ContractId == id);
                //if (contract == null) 
                //{ 
                //contract.Render.Contracts = null;
                //contract.Render.Places = null;
                //contract.Place.Contracts = null;
                //contract.Place.Owner.Contracts = null;
                //contract.Place.Owner.Places = null;
                //}
                return contract;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Contract> GetContracts()
        {
            var contracts = _context.Contracts.Include(c => c.Render).Include(c => c.Place).ThenInclude(c => c.Owner).ToList();
            //foreach (var contract in contracts)
            //{
            //    contract.Render.Contracts = null;
            //    contract.Render.Places = null;
            //    contract.Place.Contracts = null;
            //    contract.Place.Owner.Contracts = null;
            //    contract.Place.Owner.Places = null;
            //}
            return contracts;
        }
    }
}
