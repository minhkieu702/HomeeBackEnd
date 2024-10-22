using AutoMapper;
using Homee.BusinessLayer.Commons;
using Homee.BusinessLayer.Helpers;
using Homee.BusinessLayer.IServices;
using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using Homee.DataLayer.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Homee.API.Controllers
{
    [EnableCors("AllowAnyOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly HomeedbContext _context;
        private readonly IPostService _service;

        public PostController(IPostService service, HomeedbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            _service = service;
        }
        /// <summary>
        /// create post based on existed room
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost("CreateBaseOnRoom")]
        public async Task<IActionResult> Create([FromBody] PostRequest post)
        {
            if (CheckValidatationBeforePost())
            {
                return Ok(new HomeeResult(Const.FAIL_CREATE_CODE, "No paying"));
            }
            using (var transaction = _context.Database.BeginTransaction())
                {
                try
                {
                    var result = _mapper.Map<Post>(post);
                    result.RoomId = post.RoomId;
                    result.IsBlock = false;
                    result.PostedDate = DateTime.Now;
                    await _context.Posts.AddAsync(result);
                    if (_context.SaveChanges() < 1)
                    {
                        transaction.Rollback();
                        return BadRequest(new HomeeResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG));
                    }
                    foreach (var img in post.ImageUrls)
                    {
                        await _context.Images.AddAsync(new Image
                        {
                            ImageUrl = img.ImageUrl,
                            No = img.No,
                            PostId = result.PostId,
                        });
                    }
                    if (_context.SaveChanges() < 1)
                    {
                        transaction.Rollback();
                        return BadRequest(new HomeeResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG));
                    }
                    transaction.Commit();
                    return Ok(new HomeeResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, post ));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(new HomeeResult(Const.ERROR_EXCEPTION, ex.Message));
                }   
                }
        }

        private bool CheckValidatationBeforePost()
        {
            try
            {
                var claim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (claim == null || !int.TryParse(claim.Value, out int uid)) return false;

                var account = _context.Accounts
                    .Include(c => c.Orders)
                    .FirstOrDefault(c => c.AccountId == uid);
                
                if (account == null || account.Orders == null || account.Orders.Count == 0) return false;
                
                var order = account.Orders
                    .FirstOrDefault(o => o.ExpiredAt < DateTime.Now || o.ExpiredAt == null);
                
                if (order == null) return false;
                
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll() => Ok(_service.GetAll().Result);

        [Authorize]
        [HttpGet("GetByAccountId")]
        public IActionResult GetByAccountId() => Ok(_service.GetByCurrentUser(User).Result);
        
        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id) => Ok(_service.GetById(id).Result);
        
        /// <summary>
        /// update only post
        /// </summary>
        /// <param name="id"></param>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPut("UpdateBaseOnRoom/{id}")]
        public IActionResult Update(int id, [FromBody] PostRequest post) => Ok(_service.Update(id, post).Result);

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id) => Ok(_service.Delete(id).Result);

        /// <summary>
        /// input all data from place, room, post
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [Authorize]
        public IActionResult Publish([FromBody] PlacePostRequest model)
        {
            if (CheckValidatationBeforePost())
            {
                return Ok(new HomeeResult(Const.FAIL_CREATE_CODE, "No paying"));
            }
            return Ok(_service.PublishPost(model, User).Result);
        }

        /// <summary>
        /// update place room post 
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        public IActionResult Update(int postId, [FromBody] PlacePostRequest model) => Ok(_service.UpdatePlacePost(postId, model).Result);

        [HttpPost("CreateBasedOnPlace")]
        public async Task<IActionResult> CreateBasedOnPlace([FromBody]RoomPostRequest model)
        {
            if (CheckValidatationBeforePost())
            {
                return Ok(new HomeeResult(Const.FAIL_CREATE_CODE, "No paying"));
            }
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    var place = _context.Places.FirstOrDefault(c => c.PlaceId == model.PlaceId);
                    if (place == null)
                    {
                        await trans.RollbackAsync();
                        return BadRequest(new HomeeResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG));
                    }

                    var room = _mapper.Map<Room>(model);
                    await _context.Rooms.AddAsync(room);
                    if (_context.SaveChangesAsync().Result < 1)
                    {
                        await trans.RollbackAsync();
                        return BadRequest(new HomeeResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG));
                    }

                    var post = _mapper.Map<Post>(model);
                    post.RoomId = room.RoomId;
                     
                    await _context.Posts.AddAsync(post);
                    if (_context.SaveChangesAsync().Result < 1)
                    {
                        await trans.RollbackAsync();
                        return BadRequest(new HomeeResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG));
                    }

                    foreach (var imageReq in model.ImageUrls)
                    {
                        var image = _mapper.Map<Image>(imageReq);
                        image.PostId = post.PostId;
                        await _context.Images.AddAsync(image);
                    }

                    if (_context.SaveChangesAsync().Result < 1)
                    {
                        await trans.RollbackAsync();
                        return BadRequest(new HomeeResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG));
                    }

                    await trans.CommitAsync();

                    return Ok(new HomeeResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, _mapper.Map<PostResponse>(post)));
                }
                catch (Exception ex)
                {
                    await trans.RollbackAsync();
                    return BadRequest(new HomeeResult(Const.ERROR_EXCEPTION, ex.Message));
                }
            }
        }
    }
}
