using Business.Abstract;
using Core.Entities;
using Entities.Reservations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;

[ApiController]
[Route("api/reservations")]
public class ReservationController : BaseController
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public IActionResult GetAll()
    {
        var result = _reservationService.GetAll();
        return result.Success
            ? Ok(result)
            : BadRequest(result.Message);
    }

    [HttpGet("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Customer")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var result = await _reservationService.GetByIdAsync(id);

        if (!result.Success)
            return BadRequest(result.Message);

        return result.Data.CustomerId != GetCurrentUserId() && !User.IsInRole("Admin")
                ? Unauthorized()
                : Ok(result);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Customer")]
    public async Task<IActionResult> AddAsync(CreateReservationDto reservation)
    {
        if(reservation.CustomerId != GetCurrentUserId() && !User.IsInRole("Admin"))
            return Unauthorized();

        var result = await _reservationService.Reserve(reservation);
        return result.Success
            ? Ok(result)
            : BadRequest(result.Message);
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Customer")]
    public async Task<IActionResult> Update(UpdateReservationDto reservation)
    {
        if (reservation.CustomerId != GetCurrentUserId() && !User.IsInRole("Admin"))
            return Unauthorized();
        var result = await _reservationService.Update(reservation);
        return result.Success
            ? Ok(result)
            : BadRequest(result.Message);
    }

    [HttpDelete]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Customer")]
    public async Task<IActionResult> Remove(RemoveReservationDto reservation)
    {
        var reservationCustomerId= _reservationService.GetByIdAsync((int)reservation.Id!).Result.Data!.CustomerId;
        if (reservationCustomerId != GetCurrentUserId() && !User.IsInRole("Admin"))
            return Unauthorized();

        var result = await _reservationService.Remove(reservation);
        return result.Success
            ? Ok(result)
            : BadRequest(result.Message);
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Customer")]
    public async Task<IActionResult> RemoveById(int id)
    {
        var reservationCustomerId = _reservationService.GetByIdAsync(id).Result.Data!.CustomerId;
        if (reservationCustomerId != GetCurrentUserId() && !User.IsInRole("Admin"))
            return Unauthorized();

        var result = await _reservationService.RemoveById(id);
        return result.Success
            ? Ok(result)
            : BadRequest(result.Message);
    }

    [HttpGet("pagination")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public IActionResult GetAllPagination([FromQuery] BasePaginationRequest req)
    {
        var result = _reservationService.GetAllPagination(req);
        return result.Success
            ? Ok(result)
            : BadRequest(result.Message);
    }
    [HttpGet("cancel-reservation")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Customer")]
    public async Task<IActionResult> CancelReservation(int reservationId)
    {
        var reservationCustomerId = _reservationService.GetByIdAsync(reservationId).Result.Data!.CustomerId;
        if (reservationCustomerId != GetCurrentUserId() && !User.IsInRole("Admin"))
            return Unauthorized();

        var result = await _reservationService.CancelReservationById(reservationId);
        return result.Success
            ? Ok(result)
            : BadRequest(result.Message);
    }
}
