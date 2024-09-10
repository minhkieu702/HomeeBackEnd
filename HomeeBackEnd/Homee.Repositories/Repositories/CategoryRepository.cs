using Homee.DataLayer.Models;
using Homee.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.Repositories.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly HomeeDbContext _context;
        public CategoryRepository()
        {
            
        }
        public CategoryRepository(HomeeDbContext homeeDbContext)
        {
            _context = homeeDbContext;
        }
        //public int CanUpdate (int id, string name, out Category cate)
        //{
        //    var category = GetAll().FirstOrDefault(c => c.CategoryId == id);
        //    if (category == null)
        //    {
        //        cate = null;
        //        return -1;
        //    }

        //    if (category.CategoryName.ToUpper().Trim().Equals(name.ToUpper().Trim())) return 0;
        //    return 1;
        //}

        public Category GetCategory(int id)
        {
            return GetCategories().FirstOrDefault(c => c.CategoryId == id);
        }

        public List<Category> GetCategories()
        {
            var cates = _context.Categories.Include(c => c.CategoryPlaces).ThenInclude(c => c.Place).ToList();
            foreach (var cate in cates)
            {
                foreach (var categoryPlace in cate.CategoryPlaces)
                {
                    categoryPlace.Category = null;
                    categoryPlace.Place.CategoryPlaces = null;
                }
            }
            return cates;
        }
    }
}
