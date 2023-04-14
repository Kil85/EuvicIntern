using AutoMapper;
using EuvicIntern.Entities;
using EuvicIntern.Models;
using EuvicIntern.Services;
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
            _accountService.Register(registerUser);

            return Ok();
        }

    }
}
