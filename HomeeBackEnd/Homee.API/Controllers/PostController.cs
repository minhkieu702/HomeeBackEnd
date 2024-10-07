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
        [Authorize]
        [HttpPost("CreateBaseOnRoom")]
        public IActionResult Create([FromBody] PostRequest post) => Ok(_service.Create(post).Result);
        [Authorize]
        [HttpGet("GetAll")]
        public IActionResult GetAll() => Ok(_service.GetAll().Result);

        [HttpGet("GetByAccountId")]
        public IActionResult GetByAccountId() => Ok(_service.GetByCurrentUser(User).Result);
        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id) => Ok(_service.GetById(id).Result);
        
        /// <summary>
        /// update post base on existed room
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
        public IActionResult Publish([FromBody] PlacePostRequest model) => Ok(_service.PublishPost(model, User).Result);

        [HttpPost("Update")]
        public IActionResult Update(int postId, [FromBody] PlacePostRequest model) => Ok(_service.UpdatePlacePost(postId, model).Result);

        [HttpPost("CreateBasedOnPlace")]
        public async Task<IActionResult> CreateBasedOnPlace([FromBody]RoomPostRequest model)
        {
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
