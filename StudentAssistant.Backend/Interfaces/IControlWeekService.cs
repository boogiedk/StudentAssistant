﻿using System.Threading;
using System.Threading.Tasks;
using StudentAssistant.Backend.Models.ControlWeek;
using StudentAssistant.Backend.Models.ControlWeek.ViewModels;

namespace StudentAssistant.Backend.Interfaces
{
    public interface IControlWeekService
    {
        /// <summary>
        /// Получить расписание зачетной недели.
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        Task<ControlWeekViewModel> Get(ControlWeekRequestModel requestModel);

        /// <summary>
        /// Скачать свежее расписание.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DownloadAsync(CancellationToken cancellationToken);
    }
}