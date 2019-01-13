using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAssistant.Backend.Models.UserSupport;
using StudentAssistant.Backend.Services;

namespace StudentAssistant.Backend.Controllers
{
    [Produces("application/json")]
    [Route("api/usersupport")]
    public class UserSupportController : ControllerBase
    {
        private readonly IUserSupportService _userSupportService;
        private readonly IValidationService _validationService;

        public UserSupportController(IUserSupportService userSupportService, IValidationService validationService)
        {
            _userSupportService = userSupportService;
            _validationService = validationService;
        }


        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("sendfeedback")]
        public IActionResult SendFeedback(UserFeedbackRequestModel input)
        {
            try
            {
                if (input == null)
                {
                    return BadRequest("Запрос не содержит данных.");
                }

                var resultValidationServiceModel = _validationService.ValidateRequest(input);

                if(resultValidationServiceModel.Any())
                {
                    return BadRequest(resultValidationServiceModel);
                }

                var result = _userSupportService.SendFeedback(input);

                return Ok(result);
            }
            catch (Exception ex)
            {
                // log
                return BadRequest(ex);
            }
        }

    }
}