using AutoMapper;
using EuvicIntern.Entities;
using EuvicIntern.Exceptions;
using EuvicIntern.Models;
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
    }

    public class AccountService : IAccountService
    {
        private readonly EuvicDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(
            IMapper mapper,
            EuvicDbContext dbContext,
            IPasswordHasher<User> passwordHasher,
            AuthenticationSettings authenticationSettings
        )
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
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
                throw new UserNotFoundException("Email or Password incorrect");
            }

            var isPasswordOk = _passwordHasher.VerifyHashedPassword(
                user,
                user.HashedPassword,
                loginDto.Password
            );

            if (isPasswordOk == PasswordVerificationResult.Failed)
            {
                throw new UserNotFoundException("Email or Password incorrect");
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
    }
}
