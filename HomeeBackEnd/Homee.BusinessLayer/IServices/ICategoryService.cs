using Homee.BusinessLayer.Commons;
using Homee.BusinessLayer.RequestModels;
using Homee.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.BusinessLayer.IServices
{
    public interface ICategoryService
    {
        Task<IHomeeResult> GetById(int categoryId);
        Task<IHomeeResult> GetByName(string categoryName);
        Task<IHomeeResult> GetAll();
        Task<IHomeeResult> Insert(CategoryRequest category);
        Task<IHomeeResult> Update(int id, CategoryRequest category);
        Task<IHomeeResult> Delete(int id);
    }
}
