using Homee.BusinessLayer.Commons;
using Homee.BusinessLayer.Helpers;
using Homee.BusinessLayer.IServices;
using Homee.DataLayer.RequestModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;

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

        [HttpPost("Create")]
        public IActionResult Create([FromBody] int subscriptionId) => Ok(_service.Create(subscriptionId, HttpContext).Result);

        [HttpGet("GetAll")]
        public IActionResult GetAll() => Ok(_service.GetAll().Result);

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id) => Ok(_service.GetById(id));

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromBody] OrderRequest category) => Ok(_service.Update(id, category).Result);

        [HttpDelete("Delete/{id}")]
        public IActionResult Block(int id) => Ok(_service.Delete(id).Result);

        //code=00&id=2e4acf1083304877bf1a8c108b30cccd&cancel=true&status=CANCELLED&orderCode=803347
        [HttpGet("return-url")]
        public async Task<IActionResult> ExecutePayment()
        {
            var result = new ReturnUrlRequest();
            result.Code = int.Parse(HttpContext.Request.Query["code"].ToString());
            result.Id = HttpContext.Request.Query["id"].ToString();
            result.Cancel = bool.Parse(HttpContext.Request.Query["cancel"].ToString());
            result.OrderCode = int.Parse(HttpContext.Request.Query["ordercode"].ToString());
            result.Status = HttpContext.Request.Query["status"].ToString();
            return Ok(await _service.ExecutePayment(result));
        }
    }
}
