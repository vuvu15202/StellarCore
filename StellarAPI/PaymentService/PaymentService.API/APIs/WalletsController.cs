using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.Usecases;
using PaymentService.Application.Responses;
using Stellar.Shared.Models;

namespace PaymentService.API.APIs
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletsController : ControllerBase
    {
        private readonly WalletService _walletService;
        private readonly HeaderContext _headerContext;

        public WalletsController(WalletService walletService, HeaderContext headerContext)
        {
            _walletService = walletService;
            _headerContext = headerContext;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyWallet()
        {
            if (_headerContext.UserId == null) return Unauthorized();

            var wallet = await _walletService.GetWalletAsync(_headerContext.UserId.Value);
            return Ok(wallet);
        }
    }
}
