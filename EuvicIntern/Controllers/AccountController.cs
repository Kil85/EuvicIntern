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
        [Authorize]
        public ActionResult Register([FromBody] RegisterUserDto registerUser)
        {
            _accountService.Register(registerUser);

            return Ok();
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto login)
        {
            var jwtToken = _accountService.Login(login);

            return Ok(jwtToken);
        }
    }
}
