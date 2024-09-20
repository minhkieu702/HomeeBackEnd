﻿using Homee.BusinessLayer.Commons;
using Homee.DataLayer.RequestModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.BusinessLayer.IServices
{
    public interface IOrderService
    {
        Task<IHomeeResult> GetAll();
        Task<IHomeeResult> GetById(int id);
        Task<IHomeeResult> Update(int id, OrderRequest model);
        Task<IHomeeResult>Create(int model);
        Task<IHomeeResult> Delete(int id);
        Task<IHomeeResult> ExecutePayment(ReturnUrlRequest result, HttpContext httpContext);
    }
}
