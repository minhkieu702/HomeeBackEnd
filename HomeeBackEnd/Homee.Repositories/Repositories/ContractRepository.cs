using Homee.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.Repositories.Repositories
{
    public class ContractRepository : BaseRepository<Contract>, IContractRepository
    {
    }
}
