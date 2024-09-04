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
using System.Text;
using System.Threading.Tasks;

namespace Homee.Repositories.Repositories
{
    public class PlaceRepository : BaseRepository<Place>, IPlaceRepository
    {
        private readonly HomeeDbContext _context;

        public PlaceRepository()
        {
            
        }
        public PlaceRepository(HomeeDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CanInsert(PlaceRequest model)
        {
            string address = model.Province.Trim() + model.Distinct.Trim() + model.Ward.Trim() + model.Street.Trim() + model.Number.Trim();
            var place = _context.Places.FirstOrDefault(p => address.ToUpper().Equals(p.Province.Trim()+p.Distinct.Trim()+model.Ward.Trim()+model.Street.Trim()+model.Number.Trim()));
            return place == null;
        }

        public Place GetPlace(int id)
        {
            var places = _context.Places.Where(c => c.PlaceId == id).Include(p => p.CategoryPlaces).ThenInclude(c => c.Category).ToList();
            foreach (var place in places)
            {
                foreach (var item in place.CategoryPlaces)
                {
                    item.Category.CategoryPlaces = null;
                    item.Place = null;
                }
            }
            return places.FirstOrDefault();
        }

        public List<Place> GetPlaces()
        {
            var places = _context.Places.Include(p => p.CategoryPlaces).ThenInclude(c => c.Category).ToList();
            foreach (var place in places)
            {
                foreach (var item in place.CategoryPlaces)
                {
                    item.Category.CategoryPlaces = null;
                    item.Place = null;
                }
            }
            return places;
        }
        private int GetUserId(HttpContext httpContext)
        {
            try
            {
                var user = SupportingFeature.Instance.GetValueFromSession("user", httpContext);
                var userid = 1;
                if (user != null)
                {
                    userid = int.Parse(user.ToString());
                }
                return userid;
            }
            catch (Exception)
            {
                return 1;
                throw;
            }
        }
        public async Task<int> InsertPlace(PlaceRequest model, HttpContext httpContext)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    var place = new Place
                    {
                        Province = model.Province,
                        Ward = model.Ward,
                        Number = model.Number,
                        Area = model.Area,
                        Street = model.Street,
                        Rent = model.Rent,
                        Direction = model.Direction,
                        Distinct = model.Distinct,
                        NumberOfBedroom = model.NumberOfBedroom,
                        NumberOfToilet = model.NumberOfToilet,
                        OwnerId = GetUserId(httpContext)
                    };

                    // Add the Place entity to the context
                    _context.Places.Add(place);
                    var check = await _context.SaveChangesAsync();
                    if (check <= 0)
                    {
                        transaction.Rollback();
                    }
                    bool flag = false;
                    foreach (var cid in model.Categories)
                    {
                        var cate = _context.Categories.FirstOrDefault(c => c.CategoryId == cid);
                        if (cate != null)
                        {
                            _context.CategoryPlaces.Add(new CategoryPlace { CategoryId = cid, PlaceId = place.PlaceId });
                            flag = true;
                        }
                    }
                    if (flag)
                    {
                        check = await _context.SaveChangesAsync();
                        if (check <= 0)
                        {
                            transaction.Rollback();
                        }
                    }
                    if (check >0)
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
        public async Task<int> UpdatePlace(Place oldPlace, PlaceRequest newPlace, HttpContext httpContext)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var place = new Place
                    {
                        IsBlock = oldPlace.IsBlock,
                        PlaceId = oldPlace.PlaceId,
                        Province = newPlace.Province,
                        Ward = newPlace.Ward,
                        Number = newPlace.Number,
                        Area = newPlace.Area,
                        Street = newPlace.Street,
                        Rent = newPlace.Rent,
                        Direction = newPlace.Direction,
                        Distinct = newPlace.Distinct,
                        NumberOfBedroom = newPlace.NumberOfBedroom,
                        NumberOfToilet = newPlace.NumberOfToilet,
                        OwnerId = GetUserId(httpContext)
                    };
                    oldPlace = place;
                    _context.Places.Update(oldPlace);
                    var check = await _context.SaveChangesAsync();
                    if (check <= 0)
                    {
                        transaction.Rollback();
                    }
                    bool flag = false;
                    foreach (var cid in newPlace.Categories)
                    {
                        var cate = _context.Categories.FirstOrDefault(c => c.CategoryId == cid);
                        if (cate != null)
                        {
                            _context.CategoryPlaces.Add(new CategoryPlace { CategoryId = cid, PlaceId = place.PlaceId });
                            flag = true;
                        }
                    }
                    if (flag)
                    {
                        check = await _context.SaveChangesAsync();
                        if (check <= 0)
                        {
                            transaction.Rollback();
                        }
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
