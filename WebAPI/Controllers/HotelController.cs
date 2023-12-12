using Business.Abstract;
using Core.Entities;
using Entities.Hotels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : BaseController
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
                ? Ok(result)
                : BadRequest(result.Message);
        }
        [HttpGet("get-all-with-images")]
        public IActionResult GetAllWithImages()
        {
            var result = _hotelService.GetAllWithImages();
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }
        [HttpGet("get-all-for-dropdown")]
        public IActionResult GetAllForDropdown()
        {
            var result = _hotelService.GetAllForDropdown();
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }
        [HttpGet("get-by-id-with-images/{id}")]
        public IActionResult GetByIdWithImages(int id)
        {
            var result = _hotelService.GetByIdWithImages(id);
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }
        [HttpGet("get-by-id-with-images-and-rooms/{id}")]
        public IActionResult GetByIdWithImagesAndRooms(int id)
        {
            var result = _hotelService.GetByIdWithImagesAndRooms(id);
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }
        [HttpGet("change-hotel-status")]
        public async Task<IActionResult> ChangeHotelStatus(int hotelId)
        {
            var result =await _hotelService.ChangeHotelStatus(hotelId);
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _hotelService.GetByIdAsync(id);
            return result.Success
                ? Ok(result)
                : NotFound(result.Message);
        }

        [HttpPost("pagination")]
        public IActionResult GetAllPagination([FromBody] BasePaginationRequest req)
        {
            var result = _hotelService.GetAllPagination(req);
            return result.Success
                 ? Ok(result)
                 : BadRequest(result.Message);
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> AddAsync([FromForm] CreateHotelDto hotel)
        {
            var result = await _hotelService.AddAsync(hotel);
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }

        [HttpPost("add-range")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> AddRangeAsync([FromBody] List<CreateHotelDto> hotels)
        {
            var result = await _hotelService.AddRangeAsync(hotels);
            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> Update([FromForm] UpdateHotelDto hotel)
        {
            var result = await _hotelService.UpdateAsync(hotel);
            return result.Success
                ? Ok(result)
                : BadRequest(result);
        }

        [HttpPut("update-range")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdateRange([FromBody] List<UpdateHotelDto> hotels)
        {
            var result = await _hotelService.UpdateRange(hotels);
            return result.Success
                 ? Ok(result)
                 : BadRequest(result.Message);
        }

        [HttpDelete()]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> SoftRemove(RemoveHotelDto hotel)
        {
            var result = await _hotelService.SoftRemoveAsync(hotel);
            return result.Success
                 ? Ok(result)
                 : BadRequest(result.Message);
        }

        [HttpDelete("[action]/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> SoftRemoveById(int id)
        {
            var result = await _hotelService.SoftRemoveAsyncById(id);
            return result.Success
                 ? Ok(result)
                 : BadRequest(result.Message);
        }
        [HttpDelete("[action]/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> RemoveById(int id)
        {
            var result = await _hotelService.RemoveAsyncById(id);
            return result.Success
                 ? Ok(result)
                 : BadRequest(result.Message);
        }

        [HttpDelete("remove-range")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> RemoveRange([FromBody] List<RemoveHotelDto> hotels)
        {
            var result = await _hotelService.RemoveRange(hotels);
            return result.Success
                ? Ok(result.Data)
                : BadRequest(result.Message);
        }
    }
}
