using Business.Abstract;
using Core.Entities;
using Entities.Reservations;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/reservations")]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _reservationService.GetAll();
        return result.Success
            ? Ok(result)
            : BadRequest(result.Message);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var result = await _reservationService.GetByIdAsync(id);
        return result.Success
            ? Ok(result)
            : BadRequest(result.Message);
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync(CreateReservationDto reservation)
    {
        var result = await _reservationService.Reserve(reservation);
        return result.Success
            ? Ok(result)
            : BadRequest(result.Message);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateReservationDto reservation)
    {
        var result = await _reservationService.Update(reservation);
        return result.Success
            ? Ok(result)
            : BadRequest(result.Message);
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(RemoveReservationDto reservation)
    {
        var result = await _reservationService.Remove(reservation);
        return result.Success
            ? Ok(result)
            : BadRequest(result.Message);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveById(int id)
    {
        var result = await _reservationService.RemoveById(id);
        return result.Success
            ? Ok(result)
            : BadRequest(result.Message);
    }

    [HttpDelete("removerange")]
    public async Task<IActionResult> RemoveRange(List<RemoveReservationDto> reservations)
    {
        var result = await _reservationService.RemoveRange(reservations);
        return result.Success
            ? Ok(result)
            : BadRequest(result.Message);
    }

    [HttpGet("pagination")]
    public IActionResult GetAllPagination([FromQuery] BasePaginationRequest req)
    {
        var result = _reservationService.GetAllPagination(req);
        return result.Success
            ? Ok(result)
            : BadRequest(result.Message);
    }
    [HttpGet("cancel-reservation")]
    public async Task<IActionResult> CancelReservation(int reservationId)
    {
        var result =await _reservationService.CancelReservationById(reservationId);
        return result.Success
            ? Ok(result)
            : BadRequest(result.Message);
    }
}
