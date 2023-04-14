using AutoMapper;
using EuvicIntern.Entities;
using EuvicIntern.Models;
using Microsoft.AspNetCore.Identity;

namespace EuvicIntern.Services
{
    public interface IAccountService
    {
        void Register(RegisterUserDto userDto);
    }
    public class AccountService : IAccountService
    {
        private readonly EuvicDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountService(IMapper mapper, EuvicDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public void Register(RegisterUserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            var hashedPassword = _passwordHasher.HashPassword(user, userDto.Password);
            user.HashedPassword = hashedPassword;

            _dbContext.Add(user);
            _dbContext.SaveChanges();
        }
    }
}
