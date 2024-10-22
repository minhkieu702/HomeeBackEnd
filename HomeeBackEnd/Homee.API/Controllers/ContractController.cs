using Homee.BusinessLayer.IServices;
using Homee.BusinessLayer.Services;
using Homee.DataLayer.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Homee.API.Controllers
{
    [EnableCors("AllowAnyOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContractController : ControllerBase
    {
        private readonly IContractService _service;

        public ContractController(IContractService contractService)
        {
            _service = contractService;
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] ContractRequest contract) => Ok(_service.Create(contract, User).Result);

        [HttpGet("GetAll")]
        public IActionResult GetAll() => Ok(_service.GetAll().Result);

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id) => Ok(_service.GetById(id).Result);

        /// <summary>
        /// update duration
        /// </summary>
        /// <param name="id"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromBody] long duration) => Ok(_service.Update(id, duration).Result);

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id) => Ok(_service.Delete(id).Result);

        [HttpPatch("Confirming/{id}")]
        public IActionResult Confirm(int id) => Ok(_service.Confirm(id).Result);

        [HttpGet("GetByCurrentUser")]
        public IActionResult GetByCurrentUser() => Ok(_service.GetByCurrentUser(User));
    }
}
