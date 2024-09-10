using Homee.BusinessLayer.IServices;
using Homee.DataLayer.RequestModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Homee.API.Controllers
{
    [EnableCors("AllowAnyOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class InteriorController : ControllerBase
    {
        private readonly IInteriorService _placeService;

        public InteriorController(IInteriorService placeService)
        {
            _placeService = placeService;
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] InteriorRequest category) => Ok(_placeService.Create(category).Result);

        [HttpGet("GetAll")]
        public IActionResult GetAll() => Ok(_placeService.GetAll().Result);

        [HttpGet("GetByPlaceId/{id}")]
        public IActionResult GetByPlace(int id) => Ok(_placeService.GetByPlace(id).Result);

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id) => Ok(_placeService.GetById(id));

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromBody] InteriorRequest category) => Ok(_placeService.Update(id, category).Result);

        [HttpDelete("Delete/{id}")]
        public IActionResult Block(int id) => Ok(_placeService.Delete(id).Result);
    }
}
