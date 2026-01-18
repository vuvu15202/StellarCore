using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.Requests;
using PaymentService.Application.Usecases;
using Stellar.Shared.Models;

namespace PaymentService.API.APIs
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentService.Application.Usecases.PaymentService _paymentService;
        private readonly HeaderContext _headerContext;

        public PaymentsController(PaymentService.Application.Usecases.PaymentService paymentService, HeaderContext headerContext)
        {
            _paymentService = paymentService;
            _headerContext = headerContext;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest request)
        {
            if (_headerContext.UserId == null)
            {
                return Unauthorized();
            }

            try
            {
                var response = await _paymentService.ProcessPaymentAsync(_headerContext.UserId.Value, request);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("topup")]
        public async Task<IActionResult> ProcessTopUp([FromBody] TopUpRequest request)
        {
            if (_headerContext.UserId == null) return Unauthorized();

            try
            {
                var response = await _paymentService.ProcessTopUpAsync(_headerContext.UserId.Value, request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("callback")]
        public async Task<IActionResult> HandleCallback([FromQuery] string orderId, [FromQuery] string status)
        {
            // Simplified callback. Real VNPAY/MOMO uses signed params.
            // This is just a universal webhook/return point.
            bool isSuccess = status?.ToLower() == "success" || status == "00"; // VNPAY 00 is success
            
            await _paymentService.HandleCallbackAsync(orderId, isSuccess);
            
            return Ok(new { Message = "Callback processed", OrderId = orderId, Success = isSuccess });
        }
    }
}
