using Homee.BusinessLayer.IServices;
using Homee.DataLayer.RequestModels;
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
        private readonly IPostService _service;

        public PostController(IPostService service)
        {
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
    }
}
