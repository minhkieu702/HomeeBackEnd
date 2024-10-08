using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Homee.Repositories.IRepositories
{
    public interface IPlaceRepository : IBaseRepository<Place>
    {
        Task<bool> CanInsert(PlaceRequest model);
        Place GetPlace(int id);
        List<Place> GetPlaces();
        Task<int> InsertPlace(PlaceRequest place, ClaimsPrincipal user);
        Task<int> UpdatePlace(int id, PlaceRequest newPlace, ClaimsPrincipal user);
    }
}
