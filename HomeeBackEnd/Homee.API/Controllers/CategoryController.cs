using Homee.BusinessLayer.IServices;
using Homee.BusinessLayer.RequestModels;
using Homee.DataLayer;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Homee.API.Controllers
{
    [EnableCors("AllowAnyOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] CategoryRequest category) => Ok(_categoryService.Insert(category).Result);

        [HttpGet("GetAll")]
        public IActionResult GetAll() => Ok(_categoryService.GetAll());

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id) => Ok(_categoryService.GetById(id));

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromBody] CategoryRequest category) => Ok(_categoryService.Update(id, category).Result);
    }
}
