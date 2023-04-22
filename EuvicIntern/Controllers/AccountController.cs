using AutoMapper;
using EuvicIntern.Entities;
using EuvicIntern.Models;
using EuvicIntern.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EuvicIntern.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterUserDto registerUser)
        {
            var userId = _accountService.Register(registerUser);

            return Created($"/api/account/{userId}", null);
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto login)
        {
            var jwtToken = _accountService.Login(login);

            return Ok(jwtToken);
        }

        [HttpGet("all")]
        [Authorize("admin")]
        public ActionResult GetAllUsers()
        {
            var result = _accountService.GetAllUsers();

            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult GetById([FromRoute] int id)
        {
            var user = _accountService.GetUserById(id);
            return Ok(user);
        }
    }
}
