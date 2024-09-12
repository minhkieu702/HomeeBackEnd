using Homee.BusinessLayer.Commons;
using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.BusinessLayer.IServices
{
    public interface IAccountService
    {
        Task<IHomeeResult> GetAll();
        Task<IHomeeResult> GetById(int id);
        Task<IHomeeResult> Update(int id, AccountRequest model);
        Task<IHomeeResult> Block(int id);
        Task<IHomeeResult> Create(AccountRequest model);
        Task<IHomeeResult> Login(string email, string password);
        Task<IHomeeResult> ConfirmEmaiToRegister(string email, HttpContext context);
        Task<IHomeeResult> ConfirmEmaiToGetNewPassword(string email, HttpContext context);
        Task<IHomeeResult> Register(AccountRequest model, HttpContext context);
    }
}
