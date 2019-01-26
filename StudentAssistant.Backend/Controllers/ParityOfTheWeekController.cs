using System;
using Microsoft.AspNetCore.Mvc;
using StudentAssistant.Backend.Models.ParityOfTheWeek;
using StudentAssistant.Backend.Services;

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

        [HttpGet]
        [Route("today")]
        public IActionResult GenerateParityOfTheWeek()
        {
            try
            {
                var dateTimeParam = DateTime.Now;

                // генерируем модель с данными о заданном дне.
                var parityOfTheWeekModel = _parityOfTheWeekService.GenerateDataOfTheWeek(dateTimeParam);

                // подготавливаем модель для отображения (ViewModel)
                var resultViewModel = _parityOfTheWeekService.PrepareParityOfTheWeekViewModel(parityOfTheWeekModel);

                return Ok(resultViewModel);
            }
            catch(Exception ex)
            {
                // log
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("selected-date")]
        public IActionResult GenerateParityOfTheWeek([FromBody]ParityOfTheWeekRequestModel selectedDateTime)
        {
            try
            {
                if (selectedDateTime == null)
                {
                  return BadRequest("Запрос не содержит данных.");
                }

                // достаем из модели указанную дату.
                var dateTimeParam = selectedDateTime.SelectedDateTime;

                // генерируем модель с данными о заданном дне.
                var parityOfTheWeekModel = _parityOfTheWeekService.GenerateDataOfTheWeek(dateTimeParam);

                // подготавливаем модель для отображения (ViewModel)
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
