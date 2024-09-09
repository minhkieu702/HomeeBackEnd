using Homee.BusinessLayer.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.BusinessLayer.IServices
{
    public interface IOrderService
    {
        Task<IHomeeResult> GetAll();
        Task<IHomeeResult> GetById(int id);
        Task<IHomeeResult> Update(IHomeeResult result);
        Task<IHomeeResult>Create(IHomeeResult result);
        Task<IHomeeResult> Delete(int id);
    }
}
