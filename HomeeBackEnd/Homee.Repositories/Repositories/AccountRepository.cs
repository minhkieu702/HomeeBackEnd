using Homee.BusinessLayer.Helpers;
using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using Homee.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.Repositories.Repositories
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        private readonly HomeedbContext _context;

        public AccountRepository() { }
        public AccountRepository(HomeedbContext context)
        {
            _context = context;
        }
        public bool CanInsert(AccountRequest model)
        {
            var account = _context.Accounts.Where(c => c.Email == model.Email).FirstOrDefault();
            return account != null;
        }
        public List<Account> GetAccounts()
        {
            return _context.Accounts.IncludeAll().ToList();
        }
        public Account GetAccount(int id)
        {
            return _context.Accounts.IncludeAll().FirstOrDefault(c => c.AccountId == id);
        }

    }
}
