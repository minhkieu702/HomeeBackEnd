using Homee.BusinessLayer.Commons;
using Homee.BusinessLayer.IServices;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Homee.API.Controllers
{
    [EnableCors("AllowAnyOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _service;

        public AuthController(IAccountService accountService)
        {
            _service = accountService;
        }
        [HttpGet("GetOTP/{email}")]
        public IActionResult Get(string email) => Ok(_service.ConfirmEmail(email, HttpContext).Result);
        //[HttpPost("Register")]
        //public IActionResult Login(string returnUrl) 
    }
}
