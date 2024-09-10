using Homee.BusinessLayer.IServices;
using Homee.DataLayer.RequestModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Homee.API.Controllers

{
    [EnableCors("AllowAnyOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _placeService;

        public OrderController(IOrderService placeService)
        {
            _placeService = placeService;
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] OrderRequest category) => Ok(_placeService.Create(category).Result);

        [HttpGet("GetAll")]
        public IActionResult GetAll() => Ok(_placeService.GetAll().Result);

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id) => Ok(_placeService.GetById(id));

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromBody] OrderRequest category) => Ok(_placeService.Update(id, category).Result);

        [HttpDelete("Delete/{id}")]
        public IActionResult Block(int id) => Ok(_placeService.Delete(id).Result);
    }
}
