﻿using Homee.BusinessLayer.Commons;
using Homee.DataLayer.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.BusinessLayer.IServices
{
    public interface INotificationService
    {
        Task<IHomeeResult> GetAll();
        Task<IHomeeResult> Delete(int id);
        Task<IHomeeResult> GetById(int id);
        Task<IHomeeResult> Create(NotificationRequest model);
        Task<IHomeeResult> GetByContent(string keyword);
    }
}
