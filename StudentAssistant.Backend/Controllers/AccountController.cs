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
using StudentAssistant.Backend.Services;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtTokenFactory _jwtTokenFactory;
        private readonly ApplicationDbContext _context;
        private readonly IAccountService _accountService;

        public AccountController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IJwtTokenFactory jwtTokenFactory, ApplicationDbContext context, IAccountService accountService)
        {
            _jwtTokenFactory = jwtTokenFactory ?? throw new ArgumentNullException(nameof(signInManager));
            _context = context;
            _accountService = accountService;
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
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
            var user = new IdentityUser(model.Login);
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);
            cancellationToken.ThrowIfCancellationRequested();

            await _signInManager.SignInAsync(user, true);

            if (model.ApplicationRoles == IdentityRoles.Administrator)
                return BadRequest(IdentityResult.Failed(new IdentityError
                {
                    Code = "IdentityRoleError",
                    Description = "Can't registered with role Administrator"
                }));
            
            await _userManager.AddToRoleAsync(user, model.ApplicationRoles.Humanize());
            
            var token = await _jwtTokenFactory.CreateJwtToken(user.Id);
            var response = new AccountRegisterResponse {Token = token, Success = true};

            //TODO: вынести в отдельный мидлвар
            HttpContext.Response.Cookies.Append(".AspNetCore.Application.Token", token,
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
            var response = new AccountLoginResponse {Token = token,Success = true};

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
        [ProducesResponseType(typeof(AccountGetResponseModel), 200)]
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
    
    /// <summary>
    /// Модель ответа.
    /// </summary>
    public class AccountLogoutResponseModel
    {
        /// <summary>
        /// Успешность операции.
        /// </summary>
        public bool Success { get; set; }
    }

    /// <summary>
    /// Модель ответа.
    /// </summary>
    public class AccountIsAuthenticationResponseModel
    {
        /// <summary>
        /// Успешность операции.
        /// </summary>
        public bool Success { get; set; }
    }

    /// <summary>
    /// Модель ответа.
    /// </summary>
    public class AccountGetResponseModel
    {
        /// <summary>
        /// Пользователь.
        /// </summary>
        public IdentityUserViewModel IdentityUser { get; set; }
    }

    public class IdentityUserViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactorEnabled { get; set; }
    }
}