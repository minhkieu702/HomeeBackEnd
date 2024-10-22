using Homee.BusinessLayer.Commons;
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
    public interface IOrderService
    {
        Task<IHomeeResult> GetAll();
        Task<IHomeeResult> GetById(long id);
        Task<IHomeeResult> Update(long id, OrderRequest model);
        Task<IHomeeResult>Create(int subId, ClaimsPrincipal user);
        Task<IHomeeResult> Create(ClaimsPrincipal user);
        Task<IHomeeResult> Delete(long id);
        Task<IHomeeResult> ExecutePayment(PAYOS_RETURN_URLRequest result);
        Task<IHomeeResult> GetByCurrentUser(ClaimsPrincipal user);
    }
}
