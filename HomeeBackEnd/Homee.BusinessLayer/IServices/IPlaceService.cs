using Homee.BusinessLayer.Commons;
using Homee.DataLayer.RequestModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.BusinessLayer.IServices
{
    public interface IPlaceService
    {
        Task<IHomeeResult> GetById(int id);
        Task<IHomeeResult> GetAll();
        Task<IHomeeResult> Insert(PlaceRequest model, HttpContext httpContext);
        Task<IHomeeResult> Update(int id, PlaceRequest model, HttpContext httpContext);
        Task<IHomeeResult> Block(int id);
    }
}
