using MagicShop.API.Dtos;
using MagicShop.API.Interfaces;
using MagicShop.API.Repositories;
using MagicShop.Entity.Models;

namespace MagicShop.API.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository accountRepository;
        private readonly IHashService hashService;
        private readonly ITokenService tokenService;

        public AccountService(IAccountRepository accountRepository, IHashService hashService, ITokenService tokenService)
        {
            this.accountRepository = accountRepository;
            this.hashService = hashService;
            this.tokenService = tokenService;
        }

        public async Task<string> Register(RegisterDto registerDto, CancellationToken cancellationToken)
        {
            if ((await accountRepository.GetAccount(registerDto.Login, cancellationToken)) != null)
            {
                throw new InvalidOperationException("Account already exist.");
            }

            var hashResult = hashService.GetHash(registerDto.Password);

            var model = await accountRepository.AddAccount(new Entity.Models.UserAccount
            {
                Login = registerDto.Login,
                PasswordHash = hashResult.hash,
                PasswordSalt = hashResult.salt,
                Role = Entity.Models.UserRole.Customer
            }, cancellationToken);

            return tokenService.GetToken(model);
        }

        public async Task<string> Login(LoginDto loginDto, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAccount(loginDto.Login, cancellationToken);
            if (account == null)
            {
                throw new InvalidOperationException("Account doesn't exist.");
            }

            var hashResult = hashService.GetHash(loginDto.Password, account.PasswordSalt);

            if (!account.PasswordHash.SequenceEqual(hashResult.hash))
            {
                throw new InvalidOperationException("Incorrect password.");
            }

            return tokenService.GetToken(account);
        }

        public Task<List<UserAccount>> GetUsers(CancellationToken cancellationToken)
        {
            return accountRepository.GetAccounts(cancellationToken);
        }
    }
}
