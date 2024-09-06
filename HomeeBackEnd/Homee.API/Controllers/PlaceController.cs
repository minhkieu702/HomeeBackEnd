﻿using Homee.BusinessLayer.IServices;
using Homee.DataLayer.RequestModels;
using Homee.BusinessLayer.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Homee.API.Controllers
{
    [EnableCors("AllowAnyOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceController : ControllerBase
    {
        private readonly IPlaceService _placeService;

        public PlaceController(IPlaceService placeService)
        {
            _placeService = placeService;
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] PlaceRequest category) => Ok(_placeService.Insert(category, HttpContext).Result);

        [HttpGet("GetAll")]
        public IActionResult GetAll() => Ok(_placeService.GetAll());

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id) => Ok(_placeService.GetById(id));

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromBody] PlaceRequest category) => Ok(_placeService.Update(id, category, HttpContext).Result);

        [HttpPatch("Block/{id}")]
        public IActionResult Block(int id) => Ok(_placeService.Block(id).Result);
    }
}
