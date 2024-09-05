using Homee.BusinessLayer.IServices;
using Homee.BusinessLayer.Services;
using Homee.DataLayer.RequestModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Homee.API.Controllers
{
    [EnableCors("AllowAnyOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountController(IAccountService accountService)
        {
            _service = accountService;
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] AccountRequest category) => Ok(_service.Create(category).Result);

        [HttpGet("GetAll")]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id) => Ok(_service.GetById(id));

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromBody] AccountRequest category) => Ok(_service.Update(id, category).Result);

        [HttpDelete("Delete/{id}")]
        public IActionResult Block(int id) => Ok(_service.Block(id).Result);
    }
}
