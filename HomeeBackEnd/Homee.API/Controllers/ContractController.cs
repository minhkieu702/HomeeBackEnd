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
    public class ContractController : ControllerBase
    {
        private readonly IContractService _service;

        public ContractController(IContractService contractService)
        {
            _service = contractService;
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] ContractRequest contract) => Ok(_service.Create(contract).Result);

        [HttpGet("GetAll")]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id) => Ok(_service.GetById(id));

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromBody] ContractRequest contract) => Ok(_service.Update(id, contract).Result);

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id) => Ok(_service.Delete(id).Result);
    }
}
