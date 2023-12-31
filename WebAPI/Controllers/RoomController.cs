﻿using Business.Abstract;
using Core.Entities;
using Core.Utils.Results;
using Entities.Rooms;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : BaseController
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _roomService.GetAll();
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }
        [HttpGet("get-latest-room-per-hotel")]
        public IActionResult GetAllRooms(int? roomCount=0)
        {
            var result = _roomService.GetLatestRoomsPerHotel(roomCount);
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }
        [HttpGet("get-rooms-with-images-by-hotel-id/{hotelId}")]
        public IActionResult GetAllRooms(int hotelId)
        {
            var result = _roomService.GetRoomsWithImages();
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }
        [HttpGet("get-room-by-id-with-images/{roomId}")]
        public IActionResult GetByIdWithImages(int roomId)
        {
            var result = _roomService.GetByIdWithImages(roomId);
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }
        [HttpGet("get-rooms-with-images")]
        public IActionResult GetRoomsWithImages()
        {
            var result = _roomService.GetRoomsWithImages();
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomById(int id)
        {
            var result = await _roomService.GetByIdAsync(id);
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }
        [HttpGet("change-room-status")]
        public async Task<IActionResult> ChangeRoomStatus(int roomId)
        {
            var result =await _roomService.ChangeRoomStatus(roomId);
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }
        [HttpGet("[action]/{hotelId}")]
        public IActionResult GetRoomsByHotelId(int hotelId)
        {
            var result = _roomService.GetRoomsByHotelId(hotelId);
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }

        [HttpGet("pagination")]
        public IActionResult GetAllRoomsWithPagination([FromQuery] BasePaginationRequest req)
        {
            var result = _roomService.GetAllPagination(req);
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> AddRoom([FromForm] CreateRoomDto room)
        {
            var result = await _roomService.AddAsync(room);
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }

        [HttpPost("addrange")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> AddRooms([FromBody] List<CreateRoomDto> rooms)
        {
            var result = await _roomService.AddRangeAsync(rooms);
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdateRoom([FromForm] UpdateRoomDto room)
        {
            var result = await _roomService.UpdateAsync(room);
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }

        [HttpPut("updaterange")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdateRooms([FromBody] List<UpdateRoomDto> rooms)
        {
            var result = await _roomService.UpdateRange(rooms);
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }

        [HttpDelete("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> SoftRemove([FromBody] RemoveRoomDto room)
        {
            var result = await _roomService.SoftRemoveAsync(room);
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }

        [HttpDelete("[action]/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> SoftRemoveById(int id)
        {
            var result = await _roomService.SoftRemoveAsyncById(id);
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }
        [HttpDelete("[action]/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> RemoveById(int id)
        {
            var result = await _roomService.RemoveAsyncById(id);
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }

        [HttpDelete("removerange")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> RemoveRooms([FromBody] List<RemoveRoomDto> rooms)
        {
            var result = await _roomService.RemoveRange(rooms);
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }
    }
}
