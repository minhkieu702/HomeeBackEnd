using Homee.BusinessLayer.Commons;
using Homee.BusinessLayer.Helpers;
using Homee.BusinessLayer.IServices;
using Homee.DataLayer.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Homee.API.Controllers
{
    [EnableCors("AllowAnyOrigins")]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _service;

        public AuthController(IAccountService service)
        {
            _service = service;
        }

        [HttpGet("ConfirmOTPToRegister")]
        public IActionResult Get(string otp) => Ok(_service.ConfirmOtpToRegister(otp, HttpContext).Result);

        [HttpGet("GetOTPToUpdatePassword/{email}")]
        public IActionResult GetOTPToUpdatePassword(string email) => Ok(_service.ConfirmEmaiToGetNewPassword(email, HttpContext).Result);

        [HttpPost("ConfirmOTP")]
        public IActionResult ConfirmOTP(string OTP)
        {
            try
            {
                if (SupportingFeature.GetValueFromSession("otp", out string trueotp, HttpContext) == false)
                {
                    return Ok(new HomeeResult(Const.FAIL_CREATE_CODE, "Please enter email again to get new OTP."));
                }
                if (!OTP.Equals(trueotp))
                {
                    return Ok(new HomeeResult(Const.FAIL_CREATE_CODE, "The OTP is not correct."));
                }
                HttpContext.Session.Remove(OTP);

                return Ok(new HomeeResult(Const.SUCCESS_READ_CODE, "The OTP is correct."));
            }
            catch (Exception ex)
            {
                return Ok(new HomeeResult(Const.ERROR_EXCEPTION, "Something was wrong."));
            }
        }

        [HttpPost("Register")]
        public IActionResult Register([EmailAddress] string email, string password)
        {
            return Ok(_service.Register(new AccountRequest { Email = email, Password = password }, HttpContext).Result);
        }

        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword(string password) => Ok(_service.ResetPassword(password, HttpContext).Result);

        [HttpPost("Login")]
        public IActionResult Login(string email, string password)
        {
            var result = _service.Login(email, password).Result;
            return result.Status >= 1 ? Ok(result.Data.ToString()) : Unauthorized(result);
        }

        [Authorize]
        [HttpGet("Account")]
        public IActionResult GetProfile() => Ok(_service.GetProfile(User).Result);
    }
}
