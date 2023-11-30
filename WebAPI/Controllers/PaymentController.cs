using Business.Abstract;
using Core.Entities;
using Entities.Payments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public IActionResult GetAllPayments()
        {
            var result = _paymentService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var result = await _paymentService.GetByIdAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result.Message);
        }

        [HttpGet("pagination")]
        public IActionResult GetAllPaymentsWithPagination([FromQuery] BasePaginationRequest req)
        {
            var result = _paymentService.GetAllPagination(req);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}
