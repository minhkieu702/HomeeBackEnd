using Homee.BusinessLayer.IServices;
using Homee.BusinessLayer.Services;
using Homee.DataLayer.RequestModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Homee.API.Controllers
{
    [EnableCors("AllowAnyOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _service;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _service = subscriptionService;
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] SubscriptionRequest subscription) => Ok(_service.Create(subscription).Result);

        [HttpGet("GetAll")]
        public IActionResult GetAll() => Ok(_service.GetAll().Result);

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id) => Ok(_service.GetById(id).Result);

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromBody] SubscriptionRequest subscription) => Ok(_service.Update(id, subscription).Result);

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id) => Ok(_service.Delete(id).Result);
    }
}
