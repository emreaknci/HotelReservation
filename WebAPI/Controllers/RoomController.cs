using Business.Abstract;
using Core.Entities;
using Core.Utils.Results;
using Entities.Rooms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public IActionResult GetAllRooms()
        {
            var result = _roomService.GetAll();
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

        [HttpGet("[action]/{hotelId}")]
        public IActionResult GetRoomsByHotelId(int hotelId)
        {
            var result = _roomService.GetRoomsByHotelId(hotelId);
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoom([FromBody] CreateRoomDto room)
        {
            var result = await _roomService.AddAsync(room);
            return result.Success
                ? Ok(result.Data)
                : BadRequest(result.Message);
        }

        [HttpPost("addrange")]
        public async Task<IActionResult> AddRooms([FromBody] List<CreateRoomDto> rooms)
        {
            var result = await _roomService.AddRangeAsync(rooms);
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRoom([FromBody] UpdateRoomDto room)
        {
            var result = await _roomService.Update(room);
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }

        [HttpPut("updaterange")]
        public async Task<IActionResult> UpdateRooms([FromBody] List<UpdateRoomDto> rooms)
        {
            var result = await _roomService.UpdateRange(rooms);
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveRoom([FromBody] RemoveRoomDto room)
        {
            var result = await _roomService.Remove(room);
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveRoomById(int id)
        {
            var result = await _roomService.RemoveById(id);
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }

        [HttpDelete("removerange")]
        public async Task<IActionResult> RemoveRooms([FromBody] List<RemoveRoomDto> rooms)
        {
            var result = await _roomService.RemoveRange(rooms);
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
    }
}
