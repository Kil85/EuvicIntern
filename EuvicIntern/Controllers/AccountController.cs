using AutoMapper;
using EuvicIntern.Entities;
using EuvicIntern.Models;
using Microsoft.AspNetCore.Mvc;

namespace EuvicIntern.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly EuvicDbContext _dbContext;
        private readonly IMapper _mapper;

        public AccountController(EuvicDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterUserDto registerUser)
        {
            var user = _mapper.Map<User>(registerUser);
            user.HashedPassword = registerUser.Password;

            _dbContext.Add(user);
            _dbContext.SaveChanges();
            return Ok(user);
        }

    }
}
