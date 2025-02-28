using MagicShop.Entity;
using MagicShop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MagicShop.API.Controllers
{
    public record AccountRegisterDto(
        [StringLength(50, MinimumLength = 4)] string Login,
        [StringLength(100, MinimumLength = 5)] string Password,
        [EmailAddress] string Email)
    {
        public string? Name { get; set; }

        public string? LastName { get; set; }

        public string? Address { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }
    }

    public record AccountLoginDto(
        [StringLength(50, MinimumLength = 5)] string Login,
        [StringLength(100, MinimumLength = 5)] string Password);


    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly MagicShopContext _magicShop;

        public AccountController(MagicShop.Entity.MagicShopContext magicShop)
        {
            this._magicShop = magicShop;
        }

        // api/account/register
        [HttpPost("register/{userType}")]
        public async Task<ActionResult> Register(string userType, AccountRegisterDto registerDto)
        {
            if (await _magicShop.Users.AnyAsync(u => u.Login == registerDto.Login))
            {
                return BadRequest();
            }

            AccountModel account;
            if (userType == "customer")
            {
                account = new CustomerAccountModel()
                {
                    Login = registerDto.Login,
                    Email = registerDto.Email,
                    Role = RoleType.Customer,
                    Address = registerDto.Address,
                    PhoneNumber = registerDto.PhoneNumber,
                    LastName = registerDto.LastName,
                    Name = registerDto.Name
                };
            }
            else
            {
                account = new AccountModel()
                {
                    Login = registerDto.Login,
                    Email = registerDto.Email,
                    Role = userType.ToLower() switch { "admin" => RoleType.Admin, "vendor" => RoleType.Vendor }
                };
            }

            await _magicShop.Users.AddAsync(account);
            await _magicShop.SaveChangesAsync();

            return Ok();
        }

        // api/account/login
        [HttpPost("login")]
        public async Task<ActionResult> Login(AccountLoginDto loginDto)
        {
            var user = await _magicShop.Users.SingleOrDefaultAsync(u => u.Login == loginDto.Login);
            if (user == null)
            {
                return NotFound();
            }

            if (user.Password != loginDto.Password)
            {
                return Unauthorized();
            }

            return Ok();
        }
    }
}
