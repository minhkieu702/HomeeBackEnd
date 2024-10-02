using Homee.BusinessLayer.Commons;
using Homee.BusinessLayer.Helpers;
using Homee.BusinessLayer.IServices;
using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static System.Net.WebRequestMethods;

namespace Homee.API.Controllers
{
    [EnableCors("AllowAnyOrigins")]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly HomeedbContext _context;
        private readonly IAccountService _service;

        public AuthController(IAccountService service, HomeedbContext context)
        {
            _context = context;
            _service = service;
        }

        /// <summary>
        /// After register, please checking gmail to get otp then using ConfirmOTPToRegister to enter otp
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public IActionResult Register([EmailAddress] string email, string password)
        {
            return Ok(_service.Register(new AccountRequest { Email = email, Password = password }, HttpContext).Result);
        }

        /// <summary>
        /// enter otp to confirm that you want to register
        /// </summary>
        /// <param name="otp"></param>
        /// <returns></returns>
        [HttpGet("verify-email")]
        public IActionResult Get()
        {
            try
            {
                var token = HttpContext.Request.Query["token"].ToString();
                var account = _context.Accounts.FirstOrDefault(c => c.VerificationToken.Equals(token));
                if (account == null)
                {
                    return NotFound();
                }
                account.IsVerified = true;
                _context.Accounts.Update(account);
                var check = _context.SaveChanges();
                return check > 0 ? Ok(new HomeeResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, account)) : BadRequest(new HomeeResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG));
            }
            catch (Exception ex)
            {
                return Ok(new HomeeResult(Const.ERROR_EXCEPTION, "Something was wrong."));
            }
        }

        /// <summary>
        /// using for update and reset password. Checking gmail to get otp then using ResetPassword route api
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        //[HttpGet("GetOTPToUpdatePassword/{email}")]
        //public IActionResult GetOTPToUpdatePassword(string email) => Ok(_service.ConfirmEmaiToGetNewPassword(email, HttpContext).Result);

        private IActionResult ConfirmOTP(string OTP, out bool check)
        {
            check = true;
            try
            {
                if (SupportingFeature.GetValueFromSession("otp", out string trueotp, HttpContext) == false)
                {
                    check = false;
                    return BadRequest(new HomeeResult(Const.FAIL_CREATE_CODE, "Please enter email again to get new OTP."));
                }
                if (!OTP.Equals(trueotp))
                {
                    check = false;
                    return BadRequest(new HomeeResult(Const.FAIL_CREATE_CODE, "The OTP is not correct."));
                }
                return Ok(new HomeeResult(Const.SUCCESS_READ_CODE, "The OTP is correct."));
            }
            catch (Exception ex)
            {
                return BadRequest(new HomeeResult(Const.ERROR_EXCEPTION, "Something was wrong."));
            }
        }

        /// <summary>
        /// using to enter otp and new password
        /// </summary>
        /// <param name="otp"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        //[HttpPost("ResetPassword")]
        //public IActionResult ResetPassword(string otp, string password)
        //{
        //    if (ConfirmOTP(otp, out bool check) == null || check == false)
        //    {
        //        return ConfirmOTP(otp, out check);
        //    }
        //    return Ok(_service.ResetPassword(password, HttpContext).Result);
        //}

        [HttpPost("Login")]
        public IActionResult Login(string email, string password)
        {
            var result = _service.Login(email, password).Result;
            return result.Status >= 1 ? Ok(result.Data.ToString()) : Unauthorized(result);
        }

        /// <summary>
        /// GetProfile
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("Account")]
        public IActionResult GetProfile() => Ok(_service.GetProfile(User).Result);
    }
}
