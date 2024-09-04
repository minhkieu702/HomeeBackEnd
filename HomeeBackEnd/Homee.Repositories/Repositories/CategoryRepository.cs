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
        public int CanUpdate (int id, string name)
        {
            var category = GetAll().FirstOrDefault(c => c.CategoryId == id);
            if (category == null) return -1;
            if (category.CategoryName.ToUpper().Trim().Equals(name.ToUpper().Trim())) return 0;
            return 1;
        }

        public Category GetCategory(int id)
        {
            return GetCategories().FirstOrDefault(c => c.CategoryId == id);
        }

        public List<Category> GetCategories()
        {
            var cates = _context.Categories.ToList();
            foreach (var cate in cates)
            {
                cate.CategoryPlaces = _context.CategoryPlaces.Where(c => c.CategoryId == cate.CategoryId).ToList();
                foreach (var cp in cate.CategoryPlaces)
                {
                    cp.Category = null;
                    var place = _context.Places.FirstOrDefault(c => c.PlaceId == cp.PlaceId);
                    place.CategoryPlaces = null;
                }
            }
            return cates;
        }
    }
}
