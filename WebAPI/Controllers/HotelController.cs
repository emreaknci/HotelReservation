using Business.Abstract;
using Core.Entities;
using Entities.Hotels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _hotelService.GetAll();
            return result.Success
                ? Ok(result.Data)
                : BadRequest(result.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _hotelService.GetByIdAsync(id);
            return result.Success
                ? Ok(result.Data)
                : NotFound(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] CreateHotelDto hotel)
        {
            var result = await _hotelService.AddAsync(hotel);
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }

        [HttpPost("addrange")]
        public async Task<IActionResult> AddRangeAsync([FromBody] List<CreateHotelDto> hotels)
        {
            var result = await _hotelService.AddRangeAsync(hotels);
            return result.Success
                ? CreatedAtAction(nameof(GetAll), result.Data)
                : BadRequest(result.Message);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateHotelDto hotel)
        {
            var result = await _hotelService.Update(hotel);
            return result.Success
                ? Ok(result)
                : BadRequest(result);
        }

        [HttpPut("updaterange")]
        public async Task<IActionResult> UpdateRange([FromBody] List<UpdateHotelDto> hotels)
        {
            var result = await _hotelService.UpdateRange(hotels);
            return result.Success
                 ? Ok(result)
                 : BadRequest(result.Message);
        }

        [HttpDelete()]
        public async Task<IActionResult> Remove(RemoveHotelDto hotel)
        {
            var result = await _hotelService.Remove(hotel);
            return result.Success
                 ? Ok(result)
                 : NotFound(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveById(int id)
        {
            var result = await _hotelService.RemoveById(id);
            return result.Success
                 ? Ok(result)
                 : NotFound(result.Message);
        }

        [HttpDelete("removerange")]
        public async Task<IActionResult> RemoveRange([FromBody] List<RemoveHotelDto> hotels)
        {
            var result = await _hotelService.RemoveRange(hotels);
            return result.Success
                ? Ok(result.Data)
                : BadRequest(result.Message);
        }

        [HttpPost("pagination")]
        public IActionResult GetAllPagination([FromBody] BasePaginationRequest req)
        {
            var result = _hotelService.GetAllPagination(req);
            return result.Success
                 ? Ok(result)
                 : BadRequest(result.Message);
        }
    }
}
