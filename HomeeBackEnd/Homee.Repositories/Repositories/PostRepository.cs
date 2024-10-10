using AutoMapper;
using Homee.BusinessLayer.Helpers;
using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using Homee.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Homee.Repositories.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        private readonly IMapper _mapper;
        private readonly HomeedbContext _context;

        public PostRepository() { }

        public PostRepository(HomeedbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<bool> CanInsert(PlacePostRequest model)
        {
            string address = model.Province.Trim() + model.Distinct.Trim() + model.Ward.Trim() + model.Street.Trim() + model.Number.Trim();
            var place = _context.Places.FirstOrDefault(p => address.ToUpper().Equals(p.Province.Trim() + p.Distinct.Trim() + model.Ward.Trim() + model.Street.Trim() + model.Number.Trim()));
            return place != null;
        }

        public async Task<int> DeletePost(int id)
        {
            var check = 0;
            using (var p = _context.Database.BeginTransaction())
            {
                try
                {
                    var imgs = _context.Images.Where(c => c.PostId == id);
                    if (imgs != null && imgs.Count() > 0)
                    {
                        _context.Images.RemoveRange(imgs);
                        check = await _context.SaveChangesAsync();
                        if (check <= 0)
                        {
                            await p.RollbackAsync();
                            return check;
                        }
                    }

                    var fps = _context.FavoritePosts.Where(c => c.PostId == id);
                    if (fps != null && fps.Count() > 0)
                    {
                        _context.FavoritePosts.RemoveRange();
                        check = await _context.SaveChangesAsync();
                        if (check <= 0)
                        {
                            await p.RollbackAsync();
                            return check;
                        }
                    }

                    var post = _context.Posts.FirstOrDefault(c => c.PostId == id);
                    if (post != null)
                    {
                        _context.Posts.Remove(post);
                        check = await _context.SaveChangesAsync();
                        if (check <= 0)
                        {
                            await p.RollbackAsync();
                            return check;
                        }
                    }
                    await p.CommitAsync();
                    return check;
                }
                catch (Exception)
                {
                    await p.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<Post> GetPostById(int id)
        {
            var post = await _context.Posts.Include(c => c.Room).ThenInclude(c => c.Place).ThenInclude(c => c.Owner).Include(c => c.Images)
                .FirstOrDefaultAsync(c => c.PostId == id);
            return post;
        }

        public async Task<IList<Post>> GetPosts()
        {
            var posts = await _context.Posts.Include(c => c.Room).ThenInclude(c => c.Place).ThenInclude(c => c.Owner).Include(c => c.Images)
                .ToListAsync();
            return posts;
        }

        public async Task<IEnumerable<object>> GetPosts(ClaimsPrincipal user)
        {
            var aId = user.FindFirst(ClaimTypes.NameIdentifier).Value;
            var posts = _context.Posts.Include(c => c.Room).ThenInclude(c => c.Place).Where(c => c.Room.Place.OwnerId == int.Parse(aId)).ToList();
            return posts;
        }

        public async Task<int> InsertPlacePost(PlacePostRequest model, ClaimsPrincipal user)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var aId = user.FindFirst(ClaimTypes.NameIdentifier).Value;

                    #region Insert Place
                    var place = new Place
                    {
                        Province = model.Province,
                        Ward = model.Ward,
                        Number = model.Number,
                        Street = model.Street,
                        Distinct = model.Distinct,
                        OwnerId = int.Parse(aId.ToString()),
                        IsBlock = false
                    };

                    // Add the Place entity to the context
                    await _context.Places.AddAsync(place);
                    var check = await _context.SaveChangesAsync();
                    if (check <= 0) transaction.Rollback();
                    #endregion

                    #region Insert Room
                    var room = new Room
                    {
                        RoomName = model.RoomName,
                        Direction = model.Direction,
                        Area = model.Area,
                        InteriorStatus = model.InteriorStatus,
                        IsRented = model.IsRented,
                        RentAmount = model.RentAmount,
                        WaterAmount = model.WaterAmount,
                        ElectricAmount = model.ElectricAmount,
                        ServiceAmount = model.ServiceAmount,
                        PlaceId = place.PlaceId
                    };

                    await _context.Rooms.AddAsync(room);
                    check = await _context.SaveChangesAsync();
                    if (check <= 0) transaction.Rollback();
                    #endregion

                    #region Insert Post
                    var post = new Post
                    {
                        PostedDate = DateTime.Now,
                        Note = model.Note,
                        Status = model.Status,
                        IsBlock = false,
                        RoomId = room.RoomId,
                        Title = model.Title,
                    };

                    await _context.Posts.AddAsync(post);
                    check = await _context.SaveChangesAsync();
                    if (check <= 0) transaction.Rollback();
                    #endregion

                    #region Insert Image
                    if (model.ImageUrls != null && model.ImageUrls.Count > 0)
                    {
                        foreach (var url in model.ImageUrls)
                        {
                            var img = new Image
                            {
                                ImageUrl = url.ImageUrl,
                                PostId = post.PostId,
                                No = url.No,
                            };
                            _context.Images.Add(img);
                        }
                    }
                    #endregion

                    if (model.ImageUrls != null && model.ImageUrls.Count > 0)
                    {
                        check = await _context.SaveChangesAsync();
                        if (check <= 0) transaction.Rollback();
                    }

                    if (check > 0) transaction.Commit();
                    return check;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<int> UpdatePlacePost(int id, PlacePostRequest model)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region Update Post
                    var post = await _context.Posts.FirstOrDefaultAsync(c => c.PostId == id);
                    if (post == null) return 0;

                    post.PostedDate = model.PostedDate;
                    post.Note = model.Note;
                    post.Status = model.Status;
                    post.Title = model.Title;

                    _context.Posts.Update(post);
                    #endregion

                    #region Update Images List
                    /*
                     var existingImages = await _context.Images.Where(c => c.PostId == id).ToListAsync();

    var newImageUrls = model.ImageUrls.Select(i => i.ImageUrl).ToList();

    var imgsToDelete = existingImages.Where(img => !newImageUrls.Contains(img.ImageUrl)).ToList();

    var imgsToReuse = existingImages.Where(img => newImageUrls.Contains(img.ImageUrl)).ToList();

    var existingImageUrls = existingImages.Select(img => img.ImageUrl).ToList();

    var imagesToAdd = model.ImageUrls.Where(img => !existingImageUrls.Contains(img.ImageUrl)).ToList();

    if (imgsToDelete.Count > 0)
    {
        _context.Images.RemoveRange(imgsToDelete);
    }

    foreach (var newImg in imagesToAdd)
    {
        _context.Images.Add(new Image
        {
            ImageUrl = newImg.ImageUrl,
            Number = newImg.Number,
            PostId = id,
        });
    }

    foreach (var item in imgsToReuse)
    {
        item.Number = model.ImageUrls.SingleOrDefault(c => c.ImageUrl.Equals(item.ImageUrl)).Number;
        _context.Images.Update(item);
    }
                     */
                    var existingImages = await _context.Images.Where(c => c.PostId == id).ToListAsync();

                    // Use HashSet for faster lookup
                    var newImageUrls = new HashSet<string>(model.ImageUrls.Select(i => i.ImageUrl));

                    // Images to delete (in existing but not in the new list)
                    var imgsToDelete = existingImages.Where(img => !newImageUrls.Contains(img.ImageUrl)).ToList();

                    // Images to reuse (exist in both the old and new lists)
                    var imgsToReuse = existingImages.Where(img => newImageUrls.Contains(img.ImageUrl)).ToList();

                    // Use dictionary for quick lookup by ImageUrl
                    var newImagesDict = model.ImageUrls.ToDictionary(i => i.ImageUrl);

                    // Existing image URLs
                    var existingImageUrls = new HashSet<string>(existingImages.Select(img => img.ImageUrl));

                    // Images to add (in the new list but not in the database)
                    var imagesToAdd = model.ImageUrls.Where(img => !existingImageUrls.Contains(img.ImageUrl)).ToList();

                    // Remove images that are no longer needed
                    if (imgsToDelete.Any())
                    {
                        _context.Images.RemoveRange(imgsToDelete);
                    }

                    // Add new images
                    if (imagesToAdd.Any())
                    {
                        foreach (var newImg in imagesToAdd)
                        {
                            _context.Images.Add(new Image
                            {
                                ImageUrl = newImg.ImageUrl,
                                No = newImg.No,
                                PostId = id,
                            });
                        }
                    }

                    // Update reused images
                    foreach (var item in imgsToReuse)
                    {
                        item.No = newImagesDict[item.ImageUrl].No;
                    }
                    #endregion

                    #region Update Room
                    var room = _context.Rooms.FirstOrDefault(c => c.RoomId == post.RoomId);
                    if (room == null) return 0;

                    room.RoomName = model.RoomName;
                    room.Direction = model.Direction;
                    room.Area = model.Area;
                    room.InteriorStatus = model.InteriorStatus;
                    room.IsRented = model.IsRented;
                    room.RentAmount = model.RentAmount;
                    room.WaterAmount = model.WaterAmount;
                    room.ElectricAmount = model.ElectricAmount;
                    room.ServiceAmount = model.ServiceAmount;

                    _context.Rooms.Update(room);
                    #endregion

                    #region Update Place
                    var place = _context.Places.FirstOrDefault(c => c.PlaceId == room.PlaceId);
                    if (place == null) return 0;

                    place.Province = model.Province;
                    place.Distinct = model.Distinct;
                    place.Ward = model.Ward;
                    place.Street = model.Street;
                    place.Number = model.Number;

                    _context.Places.Update(place);
                    #endregion

                    var check = await _context.SaveChangesAsync();
                    if (check <= 0)
                    {
                        await transaction.RollbackAsync();
                        return check;
                    }
                    await transaction.CommitAsync();
                    return check;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<int> UpdatePost(int id, PostRequest model)
        {
            try
            {
                var post = _context.Posts.FirstOrDefault(c => c.PostId == id);
                if (post == null)
                {
                    return 0;
                }
                _mapper.Map(model, post);
                _context.Posts.Update(post);
                var check = _context.SaveChanges();
                return check;
            }
            catch (Exception)
            {

                throw;
            }
            return 0;
        }
    }
}
