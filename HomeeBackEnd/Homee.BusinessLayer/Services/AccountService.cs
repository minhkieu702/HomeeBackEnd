using AutoMapper;
using Homee.BusinessLayer.Commons;
using Homee.BusinessLayer.IServices;
using Homee.DataLayer.RequestModels;
using Homee.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.BusinessLayer.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly AccountRepository _repo;

        public AccountService(IMapper mapper, AccountRepository accountRepository)
        {
            _mapper = mapper;
            _repo = accountRepository;
        }
        public Task<IHomeeResult> Block(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IHomeeResult> Create(AccountRequest model)
        {
            throw new NotImplementedException();
        }

        public Task<IHomeeResult> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IHomeeResult> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IHomeeResult> Login(string email, string password)
        {
            throw new NotImplementedException();
        }

        public Task<IHomeeResult> Update(int id, AccountRequest model)
        {
            throw new NotImplementedException();
        }
    }
}
