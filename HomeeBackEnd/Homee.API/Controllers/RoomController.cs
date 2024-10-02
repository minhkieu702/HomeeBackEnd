using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Homee.DataLayer.Models;
using Microsoft.AspNetCore.Cors;
using Homee.DataLayer.RequestModels;
using Homee.BusinessLayer.Commons;
using Microsoft.AspNetCore.Http.HttpResults;
using AutoMapper;
using Homee.BusinessLayer.Helpers;
using Homee.DataLayer.ResponseModels;

namespace Homee.API.Controllers
{
    [EnableCors("AllowAnyOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly HomeedbContext _context;

        public RoomController(HomeedbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("AllRooms")]
        public async Task<IActionResult> GetAllRooms()
        {
            try
            {
                var result = await _context.Rooms.IncludeAll().ToListAsync();
                return result.Count == 0 ? NotFound(new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG)) : Ok(new HomeeResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result.Select(_mapper.Map<RoomResponse>)));
            }
            catch (Exception ex)
            {
                return BadRequest(new HomeeResult(Const.ERROR_EXCEPTION, ex.Message));
            }
        }

        [HttpGet("RoomsByPlaceId")]
        public async Task<IActionResult> GetRoomByPlaceId(int id)
        {
            try
            {
                var result = await _context.Rooms.Where(p => p.PlaceId == id).IncludeAll().ToListAsync();
                return result.Count == 0 ? NotFound(new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG)) : Ok(new HomeeResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result.Select(_mapper.Map<RoomResponse>)));
            }
            catch (Exception ex)
            {
                return BadRequest(new HomeeResult(Const.ERROR_EXCEPTION, ex.Message));
            }
        }

        [HttpGet("RoomById/{id}")]
        public async Task<IActionResult> GetRoomById(int id)
        {
            try
            {
                var result = await _context.Rooms.IncludeAll().FirstOrDefaultAsync(c => c.RoomId == id);
                return result == null ? NotFound(new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG)) : Ok(new HomeeResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, _mapper.Map<RoomResponse>(result)));
            }
            catch (Exception ex)
            {
                return BadRequest(new HomeeResult(Const.ERROR_EXCEPTION, ex.Message));
            }
        }

        [HttpPost("CreateRoom")]
        public async Task<IActionResult> CreateAsync([FromBody] RoomRequest model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var room = await _context.Rooms.IncludeAll().FirstOrDefaultAsync(c => c.RoomName.ToUpper().Equals(model.RoomName.ToUpper()));
                    if (room != null) return BadRequest(new HomeeResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG));

                    await _context.Rooms.AddAsync(_mapper.Map<Room>(model));
                    var check = await _context.SaveChangesAsync();
                    if (check <= 0)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest(new HomeeResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG));
                    }

                    await transaction.CommitAsync();

                    return Ok(new HomeeResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(new HomeeResult(Const.ERROR_EXCEPTION, ex.Message));
                }
            }
        }

        [HttpPatch("BlockRoom/{id}")]
        public async Task<IActionResult> BlockAsync(int roomId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var room = await _context.Rooms.IncludeAll().FirstOrDefaultAsync(c => c.RoomId == roomId);
                    if (room == null) return BadRequest(new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG));

                    room.IsBlock = true;

                    _context.Rooms.Update(room);
                    var check = await _context.SaveChangesAsync();
                    if (check <= 0)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest(new HomeeResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG));
                    }

                    await transaction.CommitAsync();

                    return Ok(new HomeeResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(new HomeeResult(Const.ERROR_EXCEPTION, ex.Message));
                }
            }
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] RoomRequest model)
        {
            using(var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var room = await _context.Rooms.IncludeAll().FirstOrDefaultAsync(c => c.RoomId == id);
                    if (room == null) return BadRequest(new HomeeResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG));

                    _mapper.Map(model, room);

                    _context.Rooms.Update(room);
                    var check = await _context.SaveChangesAsync();
                    if (check <= 0)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest(new HomeeResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG));
                    }

                    await transaction.CommitAsync();

                    return Ok(new HomeeResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, room));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(new HomeeResult(Const.ERROR_EXCEPTION, ex.Message));
                }
            }
        }
    }
}
