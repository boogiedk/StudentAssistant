using System;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentAssistant.Backend.Models;
using StudentAssistant.Backend.Models.ViewModels;
using StudentAssistant.Backend.Services;
using StudentAssistant.Backend.Services.Implementation;


namespace StudentAssistant.Backend.Controllers
{
    [Produces("application/json")]
    [Route("api/parity")]
    public class ParityOfTheWeekController : ControllerBase
    {
        private readonly IParityOfTheWeekService _parityOfTheWeekService;

        public ParityOfTheWeekController(IParityOfTheWeekService parityOfTheWeekService)
        {
            _parityOfTheWeekService = parityOfTheWeekService;
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("today")]
        public IActionResult GenerateParityOfTheWeek()
        {
            try
            {
                var dateTimeParam = DateTime.Now;

                var parityOfTheWeekModel = _parityOfTheWeekService.GenerateDataOfTheWeek(dateTimeParam);

                var resultViewModel = _parityOfTheWeekService.PrepareParityOfTheWeekViewModel(parityOfTheWeekModel);

                return Ok(resultViewModel);
            }
            catch(Exception ex)
            {
                //log
                return BadRequest(ex);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("selected-date")]
        public IActionResult GenerateParityOfTheWeek(DateTime selectedTime)
        {
            try
            {
                var dateTimeParam = selectedTime;

                var parityOfTheWeekModel = _parityOfTheWeekService.GenerateDataOfTheWeek(dateTimeParam);

                var resultViewModel = _parityOfTheWeekService.PrepareParityOfTheWeekViewModel(parityOfTheWeekModel);

                return Ok(resultViewModel);
            }
            catch(Exception ex)
            {
                //log
                return BadRequest(ex);
            }
        }
    }
}
