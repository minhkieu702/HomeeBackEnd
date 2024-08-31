using Homee.DataLayer;
using Homee.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.Repositories.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public int CanUpdate (int id, string name)
        {
            var category = GetAll().FirstOrDefault(c => c.CategoryId == id);
            if (category == null) return -1;
            if (category.CategoryName.ToUpper().Trim().Equals(name.ToUpper().Trim())) return 0;
            return 1;
        }
    }
}
