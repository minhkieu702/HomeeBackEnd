using AutoMapper;
using Homee.BusinessLayer.Helpers;
using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using Homee.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Homee.Repositories.Repositories
{
    public class PlaceRepository : BaseRepository<Place>, IPlaceRepository
    {
        private readonly IMapper _mapper;
        private readonly HomeedbContext _context;

        public PlaceRepository()
        {

        }
        public PlaceRepository(HomeedbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<bool> CanInsert(PlaceRequest model)
        {
            string address = model.Province.Trim() + model.Distinct.Trim() + model.Ward.Trim() + model.Street.Trim() + model.Number.Trim();
            var place = _context.Places.FirstOrDefault(p => address.ToUpper().Equals(p.Province.Trim() + p.Distinct.Trim() + model.Ward.Trim() + model.Street.Trim() + model.Number.Trim()));
            return place == null;
        }

        public Place GetPlace(int id)
        {
            return _context.Places.IncludeAll(_context).FirstOrDefault(c => c.PlaceId == id);
        }

        public List<Place> GetPlaces()
        {
            return _context.Places.IncludeAll(_context).ToList();
        }
        private int GetUserId(ClaimsPrincipal user)
        {
            try
            {
                if (!int.TryParse(user.FindFirst(ClaimTypes.NameIdentifier).Value, out int uerid))
                {
                    return 0;
                }
                return uerid;
            }
            catch (Exception)
            {
                return 1;
                throw;
            }
        }
        public async Task<int> InsertPlace(PlaceRequest model, ClaimsPrincipal user)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    var place = _mapper.Map<Place>(model);
                    place.OwnerId = GetUserId(user);

                    // Add the Place entity to the context
                    _context.Places.Add(place);

                    var check = await _context.SaveChangesAsync();
                    if (check <= 0)
                    {
                        transaction.Rollback();
                    }
                    transaction.Commit();
                    return check;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        public async Task<int> UpdatePlace(int id, PlaceRequest newPlace, ClaimsPrincipal user)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var oldPlace = _context.Places.FirstOrDefault(p => p.PlaceId == id);
                    var place = _mapper.Map(newPlace, oldPlace);
                    _context.Places.Update(place);
                    var check = await _context.SaveChangesAsync();
                    if (check <= 0)
                    {
                        transaction.Rollback();
                    }
                    if (check > 0)
                    {
                        transaction.Commit();
                    }
                    return check;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
