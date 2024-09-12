using Homee.BusinessLayer.Commons;
using Homee.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.BusinessLayer.IServices
{
    public interface IMailService
    {
        Task<IHomeeResult> SendMail(string email, string subject, string htmlMessage);
    }
}
