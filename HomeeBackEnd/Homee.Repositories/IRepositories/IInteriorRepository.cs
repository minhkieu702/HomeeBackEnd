using Homee.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.Repositories.IRepositories
{
    public interface IInteriorRepository : IBaseRepository<Interior>
    {
        IEnumerable<Interior> GetByPlace(int id);
        Task<Interior> GetInterior(int id);
        IEnumerable<Interior> GetInteriors();
    }
}
