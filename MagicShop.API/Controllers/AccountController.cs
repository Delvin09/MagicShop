using MagicShop.Entity;
using MagicShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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

    public record AccountLoginResultDto(string Login, string Token);

    public interface IHashService
    {
        (byte[] hash, byte[] key) GetHash(string key, byte[]? baseKey = null);
    }

    internal class HashService : IHashService
    {
        public (byte[] hash, byte[] key) GetHash(string data, byte[]? baseKey = null)
        {
            using var hmac = baseKey != null ? new HMACSHA512(baseKey) : new HMACSHA512();
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return (hash, hmac.Key);
        }
    }


    public interface ITokenService
    {
        string CreateToken(AccountModel user);
    }

    internal class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration configuration)
        {
            var strKey = configuration["TokenKey"];
            if (strKey == null) throw new ArgumentNullException(nameof(strKey));

            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(strKey));
        }

        public string CreateToken(AccountModel user)
        {
            var claims = new List<Claim>
            {
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.NameId, user.Login),
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var signature = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var descr = new SecurityTokenDescriptor
            {
                SigningCredentials = signature,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(10),
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(descr);
            return handler.WriteToken(token);
        }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly MagicShopContext _magicShop;
        private readonly IHashService _hashService;
        private readonly ITokenService _tokenService;

        public AccountController(MagicShop.Entity.MagicShopContext magicShop, IHashService hashService, ITokenService tokenService)
        {
            this._magicShop = magicShop;
            this._hashService = hashService;
            this._tokenService = tokenService;
        }

        // api/account/register
        [HttpPost("register/{userType}")]
        [AllowAnonymous]
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
                    Role = userType.ToLower() switch
                    {
                        "admin" => RoleType.Admin,
                        "vendor" => RoleType.Vendor
                    }
                };
            }

            var hashResult = _hashService.GetHash(registerDto.Password);
            account.PasswordHash = hashResult.hash;
            account.PasswordSalt = hashResult.key;

            await _magicShop.Users.AddAsync(account);
            await _magicShop.SaveChangesAsync();

            var token = _tokenService.CreateToken(account);
            return Ok(new AccountLoginResultDto(account.Login, token));
        }

        // api/account/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(AccountLoginDto loginDto)
        {
            var user = await _magicShop.Users.SingleOrDefaultAsync(u => u.Login == loginDto.Login);
            if (user == null)
            {
                return NotFound();
            }

            var hashResult = _hashService.GetHash(loginDto.Password, user.PasswordSalt);
            if (!user.PasswordHash.SequenceEqual(hashResult.hash))
                return Unauthorized();

            var token = _tokenService.CreateToken(user);
            return Ok(new AccountLoginResultDto(user.Login, token));
        }

        [HttpGet] // api/account GET - list
        [Authorize]
        public async Task<ActionResult> GetUsers()
        {
            var result = await _magicShop.Users.ToArrayAsync();
            return Ok(result);
        }

        [HttpGet("{id}")] // api/account/3123 GET 
        [Authorize]
        public async Task<ActionResult> GetUser(int id)
        {
            var user = await _magicShop.Users.FindAsync(id);
            return Ok(user);
        }
    }
}
