using System;
using System.Net;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentAssistant.Backend.Models;
using StudentAssistant.Backend.Services;
using StudentAssistant.Backend.Services.Implementation;
using StudentAssistant.Backend.ViewModels;

namespace StudentAssistant.Backend.Controllers
{
    [Produces("application/json")]
    [Microsoft.AspNetCore.Mvc.Route("api/parity")]
    public class ParityOfTheWeekController : ApiController
    {
        private readonly IParityOfTheWeekService _parityOfTheWeekService;

        public ParityOfTheWeekController(IParityOfTheWeekService parityOfTheWeekService)
        {
            _parityOfTheWeekService = parityOfTheWeekService;
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("today")]
        public ParityOfTheWeekViewModel GenerateParityOfTheWeek()
        {
            var dateTimeParam = DateTime.Now;

            var parityOfTheWeekModel = _parityOfTheWeekService.GenerateDataOfTheWeek(dateTimeParam);

            var resultViewModel = _parityOfTheWeekService.PrepareParityOfTheWeekViewModel(parityOfTheWeekModel);

            return resultViewModel;
        }
    }
}
