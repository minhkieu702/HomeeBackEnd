﻿using Homee.BusinessLayer.Helpers;
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
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IOrderService _service;

        public OrderController(IOrderService placeService, IConfiguration configuration)
        {
            _config = configuration;
            _service = placeService;
        }
        /// <summary>
        /// paying to have subscription
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Subscription")]
        public IActionResult Create([FromBody] int subscriptionId) => Ok(_service.Create(subscriptionId, User).Result);
        /// <summary>
        /// pay to post a posting
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Posting")]
        public IActionResult Create() => Ok(_service.Create(User).Result);

        [HttpGet("GetAll")]
        public IActionResult GetAll() => Ok(_service.GetAll().Result);

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(long id) => Ok(_service.GetById(id));

        [HttpGet("GetByCurrentUser")]
        [Authorize]
        public IActionResult GetByCurrentUser() => Ok(_service.GetByCurrentUser(User).Result);

        [HttpPut("Update/{id}")]
        public IActionResult Update(long id, [FromBody] OrderRequest order) => Ok(_service.Update(id, order).Result);

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(long id) => Ok(_service.Delete(id).Result);

        //code=00&id=2e4acf1083304877bf1a8c108b30cccd&cancel=true&status=CANCELLED&orderCode=803347
        [HttpGet("return-url")]
        public IActionResult ExecutePayment()
        {
            var result = new PAYOS_RETURN_URLRequest();
            result.Code = int.Parse(HttpContext.Request.Query["code"].ToString());
            result.SubId = SupportingFeature.Instance.ExtractSubIdFromCombinedCode(long.Parse(HttpContext.Request.Query["ordercode"].ToString()));
            result.OrderCode = long.Parse(HttpContext.Request.Query["ordercode"].ToString());
            result.Cancel = bool.Parse(HttpContext.Request.Query["cancel"].ToString());
            result.PaymentId = HttpContext.Request.Query["id"].ToString();
            result.Status = HttpContext.Request.Query["status"].ToString();
            
            return Ok(_service.ExecutePayment(result).Result);
        }
    }
}
