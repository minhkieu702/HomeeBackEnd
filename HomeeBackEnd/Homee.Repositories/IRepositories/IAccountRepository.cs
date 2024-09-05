using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.Repositories.IRepositories
{
    public interface IAccountRepository : IBaseRepository<Account>
    {
        bool CanInsert(AccountRequest model);
        Account GetAccount(int id);
    }
}
