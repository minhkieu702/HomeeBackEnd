using Homee.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.Repositories.IRepositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        int CanUpdate(int id, string name);
        List<Category> GetCategories();
        Category GetCategory(int id);
    }
}
