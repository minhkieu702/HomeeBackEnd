﻿using Homee.BusinessLayer.Commons;
using Homee.BusinessLayer.IServices;
using Homee.DataLayer.RequestModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Homee.BusinessLayer.Services
{
    public class NotificationService : INotificationService
    {
        public Task<IHomeeResult> Create(NotificationRequest model)
        {
            throw new NotImplementedException();
        }

        public Task<IHomeeResult> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IHomeeResult> GetByCurrentUser(ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public Task<IHomeeResult> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IHomeeResult> GetByContent(string keyword)
        {
            throw new NotImplementedException();
        }

        public Task<IHomeeResult> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
