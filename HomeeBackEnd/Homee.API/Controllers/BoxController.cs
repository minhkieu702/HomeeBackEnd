using AutoMapper;
using Homee.DataLayer.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Homee.API.Controllers
{
    [EnableCors("AllowAnyOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class BoxController : ControllerBase
    {
        private readonly HomeedbContext _context;
        private readonly IMapper _mapper;

        public BoxController(IMapper mapper, HomeedbContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        
    }
}
