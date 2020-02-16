using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAssistant.Backend.Interfaces;
using StudentAssistant.Backend.Models.Account.Requests;
using StudentAssistant.Backend.Models.Account.Responses;
using StudentAssistant.DbLayer;
using StudentAssistant.DbLayer.Models;

namespace StudentAssistant.Backend.Controllers
{
    [Route("api/v1/account")]
    [Produces("application/json")]
    [EnableCors("CorsPolicy")]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtTokenFactory _jwtTokenFactory;
        private readonly ApplicationDbContext _context;
        private readonly IAccountService _accountService;

        public AccountController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IJwtTokenFactory jwtTokenFactory, 
            ApplicationDbContext context,
            IAccountService accountService,
            RoleManager<IdentityRole> roleManager)
        {
            _jwtTokenFactory = jwtTokenFactory;
            _context = context;
            _accountService = accountService;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        /// <summary>
        /// Метод для регистрации пользователя.
        /// POST api/v1/account/register
        /// </summary>
        /// <param name="model">Модель с данными по пользователе.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(AccountRegisterResponse), 200)]
        public async Task<IActionResult> Register(
            [FromBody] AccountRegisterRequest model,
            CancellationToken cancellationToken)
        {
           var response = await _accountService.Register(model,cancellationToken);

           if (!response.IdentityResult.Succeeded) return BadRequest(response.IdentityResult.Errors);
           
           //TODO: вынести в отдельный мидлвар
            HttpContext.Response.Cookies.Append(".AspNetCore.Application.Token", response.Token,
                new CookieOptions
                {
                    HttpOnly = true,
                    MaxAge = TimeSpan.FromDays(7),
                });

            return Ok(response);
        }

        /// <summary>
        /// Метод для авторизации и аутентификации пользователя.
        /// POST api/v1/account/login
        /// </summary>
        /// <param name="model">Модель с данными о пользователе.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AccountLoginResponse), 200)]
        public async Task<IActionResult> Login(
            [FromBody] AccountLoginRequest model,
            CancellationToken cancellationToken)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model?.Login,
                model?.Password,
                false,
                false);

            if (!result.Succeeded)
                return Unauthorized(IdentityResult.Failed(new IdentityError
                    {Code = "Unauthorized", Description = "Unauthorized"}));
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByNameAsync(model?.Login);
            var token = await _jwtTokenFactory.CreateJwtToken(user.Id);
            var response = new AccountLoginResponse {Token = token, Success = true};

            //TODO: вынести в отдельный мидлвар
            HttpContext.Response.Cookies.Append(".AspNetCore.Application.Token", token,
                new CookieOptions
                {
                    HttpOnly = true,
                    MaxAge = TimeSpan.FromDays(7)
                }
            );

            HttpContext.Response.Cookies.Append("isAuth", "true",
                new CookieOptions
                {
                    HttpOnly = false,
                    MaxAge = TimeSpan.FromDays(7)
                });

            return Ok(response);
        }

        /// <summary>
        /// Метод для получения аккаунта пользователя по логину.
        /// POST api/v1/account/get
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("get")]
        [ProducesResponseType(typeof(ProfileViewModel), 200)]
        [Authorize]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var requestUserId = (HttpContext.User.Identity as ClaimsIdentity)?.Name ?? string.Empty;

            var response = await _accountService.Get(requestUserId);

            return Ok(response);
        }
        
        /// <summary>
        /// Метод для получения флага авторизован пользователь или нет.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("isAuth")]
        [ProducesResponseType(typeof(AccountIsAuthenticationResponseModel), 200)]
        public IActionResult IsAuthentication()
        {
            /*
             * Если пользователь авторизован, в ответ придет кука с флагом <true>,
             * чтобы клиент смог воспользоваться этой информацией.
             */
            HttpContext.Response.Cookies.Append("isAuth", "true",
                new CookieOptions
                {
                    HttpOnly = false,
                    MaxAge = TimeSpan.FromDays(7)
                });

            return Ok(new AccountIsAuthenticationResponseModel
            {
                Success = true
            });
        }

        /// <summary>
        /// Метод для получения флага авторизован пользователь или нет.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("logout")]
        [ProducesResponseType(typeof(AccountLogoutResponseModel), 200)]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("isAuth");
            Response.Cookies.Delete(".AspNetCore.Application.Token");

            return Ok(new AccountLogoutResponseModel
            {
                Success = true
            });
        }
    }
}