﻿using Homee.BusinessLayer.Commons;
using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.BusinessLayer.IServices
{
    public interface IFavoritePostService
    {
        Task<IHomeeResult> GetAll();
        Task<IHomeeResult> GetById(int id);
        Task<IHomeeResult> Create(FavoritePostRequest model);
        Task<IHomeeResult> Delete(int id);
    }
}
