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
            return _context.Accounts
                //.Include(c => c.Places).ThenInclude(c => c.Contracts)
                //.Include(c => c.FavoritePosts).ThenInclude(c => c.Post)
                //.Include(c => c.Contracts).ThenInclude(c => c.Render)
                //.Include(c => c.Notifications)
                //.Include(c => c.Orders).ThenInclude(c => c.Subscription)
                .ToList();
        }
        public Account GetAccount(int id)
        {
            var account = _context.Accounts
                //.Include(c => c.Places).ThenInclude(c => c.CategoryPlaces).ThenInclude(c => c.Category)
                //.Include(c => c.Contracts)
                //.Include(c => c.FavoritePosts).ThenInclude(c => c.Post)
                //.Include(c => c.Contracts).ThenInclude(c => c.Render)
                .Include(c => c.Notifications)
                .Include(c => c.Orders).ThenInclude(c => c.Subscription)
                .ToList().FirstOrDefault(c => c.AccountId == id);
            foreach (var order in account.Orders)
            {
                order.Subscription.Orders = null;
            }
            return account;
        }

    }
}
