using AutoMapper;
using EuvicIntern.Authorization;
using EuvicIntern.Entities;
using EuvicIntern.Exceptions;
using EuvicIntern.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EuvicIntern.Services
{
    public interface IAccountService
    {
        void Register(RegisterUserDto userDto);
        string Login(LoginDto loginDto);
        IEnumerable<UserDto> GetAll();
        User GetUser(int id);
    }

    public class AccountService : IAccountService
    {
        private readonly EuvicDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly ILogger<AccountService> _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public AccountService(
            IMapper mapper,
            EuvicDbContext dbContext,
            IPasswordHasher<User> passwordHasher,
            AuthenticationSettings authenticationSettings,
            IAuthorizationService authorizationService,
            ILogger<AccountService> logger,
            IUserContextService userContextService
        )
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
            _authorizationService = authorizationService;
            _logger = logger;
            _userContextService = userContextService;
        }

        public void Register(RegisterUserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            var hashedPassword = _passwordHasher.HashPassword(user, userDto.Password);
            user.HashedPassword = hashedPassword;

            _dbContext.Add(user);
            _dbContext.SaveChanges();
        }

        public string Login(LoginDto loginDto)
        {
            var user = _dbContext.Users
                .Include(r => r.Role)
                .FirstOrDefault(u => u.Email == loginDto.Email);

            if (user == null)
            {
                throw new LoginFailedException("Email or Password incorrect");
            }

            var isPasswordOk = _passwordHasher.VerifyHashedPassword(
                user,
                user.HashedPassword,
                loginDto.Password
            );

            if (isPasswordOk == PasswordVerificationResult.Failed)
            {
                throw new LoginFailedException("Email or Password incorrect");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey)
            );

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpiredDays);

            var token = new JwtSecurityToken(
                _authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public IEnumerable<UserDto> GetAll()
        {
            var userList = _dbContext.Users.Include(u => u.Role).ToList();
            var userDtoList = _mapper.Map<List<UserDto>>(userList);

            if (userDtoList == null)
                return null;

            return userDtoList;
        }

        public User GetUser(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            var whoIsAsking = _userContextService.GetUser.Claims
                .FirstOrDefault(u => u.Type == ClaimTypes.Role)
                .ToString();

            var authorizationResult = _authorizationService
                .AuthorizeAsync(
                    _userContextService.GetUser,
                    user,
                    new GetUserRequirment(whoIsAsking)
                )
                .Result;

            if (!authorizationResult.Succeeded)
            {
                throw new AuthorizationFailedException("You don't have access to this account");
            }

            return user;
        }
    }
}
