using Homee.BusinessLayer.Helpers;
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
        private readonly HomeedbContext _context;
        public CategoryRepository()
        {
            
        }
        public CategoryRepository(HomeedbContext HomeedbContext)
        {
            _context = HomeedbContext;
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.IncludeAll().FirstOrDefault(c => c.CategoryId == id);
        }

        public List<Category> GetCategories()
        {
            return _context.Categories.IncludeAll().ToList();
        }
    }
}
