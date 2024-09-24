﻿using Homee.BusinessLayer.IServices;
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

        [HttpPut("UpdateBaseOnRoom/{id}")]
        public IActionResult Update(int id, [FromBody] PostRequest post) => Ok(_service.Update(id, post).Result);

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id) => Ok(_service.Delete(id).Result);

        [HttpPost("Create")]
        public IActionResult Publish([FromBody] PlacePostRequest model) => Ok(_service.PublishPost(model, User).Result);

        [HttpPost("Update")]
        public IActionResult Update(int postId, [FromBody] PlacePostRequest model) => Ok(_service.UpdatePlacePost(postId, model).Result);
    }
}
