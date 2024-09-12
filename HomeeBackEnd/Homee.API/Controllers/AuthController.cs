using Homee.BusinessLayer.Commons;
using Homee.BusinessLayer.Helpers;
using Homee.BusinessLayer.IServices;
using Homee.DataLayer.RequestModels;
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
        [HttpGet("GetOTPToRegister/{email}")]
        public IActionResult Get(string email) => Ok(_service.ConfirmEmaiToRegister(email, HttpContext).Result);
        [HttpGet("GetOTPToUpdatePassword/{email}")]
        public IActionResult GetOTPToUpdatePassword(string email) => Ok(_service.ConfirmEmaiToGetNewPassword(email, HttpContext).Result);
        [HttpPost("ConfirmOTP")]
        public IActionResult ConfirmOTP(string OTP)
        {
            try
            {
                if (SupportingFeature.GetValueFromSession("otp", out string trueotp, HttpContext)==false)
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
        public IActionResult Register(AccountRequest account) => Ok(_service.Register(account, HttpContext).Result);
    }
}
