using Homee.BusinessLayer.Commons;
using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.BusinessLayer.IServices
{
    public interface IInteriorService
    {
        Task<IHomeeResult> GetAll();
        Task<IHomeeResult> GetById(int id);
        Task<IHomeeResult> Update(int id, InteriorRequest model);
        Task<IHomeeResult> Delete(int id);
        Task<IHomeeResult> Create(InteriorRequest model);
        Task<IHomeeResult> GetByPlace(int placeid);
    }
}
