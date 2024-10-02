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
    public interface IContractRepository : IBaseRepository<Contract>
    {
        List<Contract> GetContracts();
        Contract GetContract(int id);
        bool CanCreate(ContractRequest request);
        List<Contract> GetContractByCurrentUser(ClaimsPrincipal user);
    }
}
