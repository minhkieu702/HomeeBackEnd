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
    public class InteriorRepository : BaseRepository<Interior>, IInteriorRepository
    {
        private readonly HomeeDbContext _context;

        public InteriorRepository()
        {
                
        }
        public InteriorRepository(HomeeDbContext homeeDbContext)
        {
            _context = homeeDbContext;
        }
        public IEnumerable<Interior> GetByPlace(int id)
        {
            var interiors = _context.Interiors.Where(c => c.PlaceId == id).Include(c => c.Place);
            foreach (var item in interiors)
            {
                if (item.Place.Interiors != null)
                {
                    item.Place.Interiors = null;
                }
            }
            return interiors;
        }

        public async Task<Interior> GetInterior(int id)
        {
            var interiors = _context.Interiors.Include(c => c.Place).FirstOrDefault(c => c.InteriorId == id);
            if (interiors.Place.Interiors != null)
            {
                interiors.Place.Interiors = null;
            }
            return interiors;
        }

        public IEnumerable<Interior> GetInteriors()
        {
            var interiors = _context.Interiors.Include(c => c.Place);
            foreach (var item in interiors)
            {
                if (item.Place.Interiors != null)
                {
                    item.Place.Interiors = null;
                }
            }
            return interiors;
        }
    }
}
