using MagicShop.API.Dtos;
using MagicShop.API.Interfaces;
using MagicShop.API.Services;
using MagicShop.Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace MagicShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<LoginDto>> Register([FromBody] RegisterDto registerDto, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _accountService.Register(registerDto, cancellationToken));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _accountService.Login(loginDto, cancellationToken));
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet("users")]
        public async Task<ActionResult> GetUsers(CancellationToken cancellationToken)
        {
            try
            {
                var accounts = await _accountService.GetUsers(cancellationToken);
                return Ok(accounts.Select(acc => new AccountDto(acc.Id, acc.Login, acc.Role)));
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
