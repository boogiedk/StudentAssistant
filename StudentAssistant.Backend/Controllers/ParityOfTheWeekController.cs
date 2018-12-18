using System;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentAssistant.Backend.Models;
using StudentAssistant.Backend.Services;
using StudentAssistant.Backend.Services.Implementation;
using StudentAssistant.Backend.ViewModels;

namespace StudentAssistant.Backend.Controllers
{
    [Produces("application/json")]
    [Route("api/parity")]
    public class ParityOfTheWeekController : Controller
    {
        private readonly IParityOfTheWeekService _parityOfTheWeekService;

        public ParityOfTheWeekController(IParityOfTheWeekService parityOfTheWeekService)
        {
            _parityOfTheWeekService = parityOfTheWeekService;
        }

        public IActionResult Index => View();

        [HttpGet]
        [Route("today")]
        public ParityOfTheWeekViewModel GenerateParityOfTheWeek()
        {
            var dateTimeParam = DateTime.Now;

            var parityOfTheWeekModel = _parityOfTheWeekService.GenerateDataOfTheWeek(dateTimeParam);

            var resultViewModel = _parityOfTheWeekService.PrepareParityOfTheWeekViewModel(parityOfTheWeekModel);

            return resultViewModel;
        }
    }
}
