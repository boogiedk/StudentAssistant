using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentAssistant.Backend.Interfaces;
using StudentAssistant.Backend.Models.Account.Requests;
using StudentAssistant.Backend.Models.Account.Responses;
using StudentAssistant.Backend.Services;

namespace StudentAssistant.Backend.Controllers
{
    [Route("api/v1/account")]
    [Produces("application/json")]
    [AllowAnonymous]
    [EnableCors("CorsPolicy")]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtTokenFactory _jwtTokenFactory;

        public AccountController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IJwtTokenFactory jwtTokenFactory)
        {
            _jwtTokenFactory = jwtTokenFactory ?? throw new ArgumentNullException(nameof(signInManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        // POST api/v1/account/register
        [HttpPost("register")]
        [ProducesResponseType(typeof(AccountRegisterResponse), 200)]
        public async Task<IActionResult> Register(
            [FromBody] AccountRegisterRequest model,
            CancellationToken cancellationToken)
        {
            var user = new IdentityUser(model?.Login);
            var result = await _userManager.CreateAsync(user, model?.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);
            cancellationToken.ThrowIfCancellationRequested();

            await _signInManager.SignInAsync(user, true);

            var token = await _jwtTokenFactory.CreateJwtToken(user.Id);
            var response = new AccountRegisterResponse { Token = token };

            return Ok(response);
        }

        // POST api/v1/account/login
        [HttpPost("login")]
        [ProducesResponseType(typeof(AccountLoginResponse), 200)]
        public async Task<IActionResult> Login(
            [FromBody] AccountLoginRequest model,
            CancellationToken cancellationToken)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model?.Login, model?.Password, false, false);

            if (!result.Succeeded) return Unauthorized();
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByNameAsync(model?.Login);
            var token = await _jwtTokenFactory.CreateJwtToken(user.Id);
            var response = new AccountLoginResponse { Token = token };

            return Ok(response);
        }
    }
}