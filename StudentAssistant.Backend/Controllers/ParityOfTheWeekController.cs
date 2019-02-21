using System;
using Microsoft.AspNetCore.Mvc;
using StudentAssistant.Backend.Models.ParityOfTheWeek;
using StudentAssistant.Backend.Models.ParityOfTheWeek.ViewModels;
using StudentAssistant.Backend.Services;

namespace StudentAssistant.Backend.Controllers
{
    /// <summary>
    /// Контроллер с методами для получения данных о дне недели.
    /// </summary>
    [Produces("application/json")]
    [Route("api/parity")]
    public class ParityOfTheWeekController : ControllerBase
    {
        private readonly IParityOfTheWeekService _parityOfTheWeekService;

        /// <summary>
        /// Основной конструктор.
        /// </summary>
        /// <param name="parityOfTheWeekService"></param>
        public ParityOfTheWeekController(IParityOfTheWeekService parityOfTheWeekService)
        {
            _parityOfTheWeekService = parityOfTheWeekService;
        }

        /// <summary>
        /// Метод для получения данных о текущем дне недели.
        /// </summary>
        /// <returns><see cref="ParityOfTheWeekViewModel"/>Модель представления.</returns>
        [HttpGet]
        [Route("today")]
        public IActionResult GenerateParityOfTheWeek()
        {
            try
            {
                /*
               * В будущем, когда будет разработан сервис авторизации и аутентификации, в качестве параметра в каждый метод контроллера будет передаваться
               * модель с данными, в том числе, данные о часовом поясе. Соответственно, цепочка вызова метода сервиса будет такова:
               * запрос клиента -> получение времени в UTC на сервере -> прибавление к времени UTC кол-во часов из пояса -> вызов метода сервиса.
               */

                var userAccountRequestData = new UserAccountRequestDataParityOfTheWeek
                {
                    TimeZoneId = "Russian Standard Time"
                };

                var dateTimeOffsetRequestUtc = DateTimeOffset.UtcNow;

                var dateTimeOffsetRequestUser = TimeZoneInfo.ConvertTime(dateTimeOffsetRequestUtc, TimeZoneInfo.FindSystemTimeZoneById(userAccountRequestData.TimeZoneId));

                // генерируем модель с данными о заданном дне.
                var parityOfTheWeekModel = _parityOfTheWeekService.GenerateDataOfTheWeek(dateTimeOffsetRequestUser);

                // подготавливаем модель для отображения (ViewModel)
                var resultViewModel = _parityOfTheWeekService.PrepareParityOfTheWeekViewModel(parityOfTheWeekModel);

                return Ok(resultViewModel);
            }
            catch (Exception ex)
            {
                // log
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Метод для получения данных о указанном дне недели.
        /// </summary>
        /// <returns><see cref="ParityOfTheWeekViewModel"/>Модель представления.</returns>
        /// <param name="selectedDateTime">Модель, содержащая выбранную дату.</param>
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
            catch (Exception ex)
            {
                //log
                return BadRequest(ex);
            }
        }
    }
}
