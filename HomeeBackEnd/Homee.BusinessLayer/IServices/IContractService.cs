using Homee.BusinessLayer.Commons;
using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Homee.BusinessLayer.IServices
{
    public interface IContractService
    {
        Task<IHomeeResult> GetAll();
        Task<IHomeeResult> GetById(int id);
        Task<IHomeeResult> Update(int id, long model);
        Task<IHomeeResult> Delete(int id);
        Task<IHomeeResult> Create(ContractRequest model, ClaimsPrincipal claimsPrincipal);
        Task<IHomeeResult> GetByCurrentUser(ClaimsPrincipal user);
        Task<IHomeeResult> Confirm(int contractId);
    }
}
