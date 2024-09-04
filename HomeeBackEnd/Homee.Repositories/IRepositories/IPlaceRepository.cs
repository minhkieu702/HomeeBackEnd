using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.Repositories.IRepositories
{
    public interface IPlaceRepository : IBaseRepository<Place>
    {
        Task<bool> CanInsert(PlaceRequest model);
        Place GetPlace(int id);
        List<Place> GetPlaces();
        Task<int> InsertPlace(PlaceRequest place, HttpContext httpContext);
        Task<int> UpdatePlace(Place oldPlace, PlaceRequest newPlace, HttpContext httpContext);
    }
}
