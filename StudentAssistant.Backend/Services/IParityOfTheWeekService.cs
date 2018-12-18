using System;
using StudentAssistant.Backend.Models;
using StudentAssistant.Backend.ViewModels;

namespace StudentAssistant.Backend.Services
{
    /// <summary>
    /// Интерфейс для работы с сервисом, связанного с формированием данных о заданном <see cref="DateTime"/> параметре.
    /// </summary>
    public interface IParityOfTheWeekService
    {
        /// <summary>
        /// Возвращает <see cref="bool"/> true, если неделя чётная, иначе <see cref="bool"/> false.
        /// </summary>
        /// <param name="timeNowParam"></param>
        /// <returns></returns>  
        bool GetParityOfTheWeekByDateTime(DateTime timeNowParam);

        /// <summary>
        /// Генерирует и возвращает <see cref="ParityOfTheWeekModel"/> модель с данными о текущем дне по заданному <see cref="DateTime"/>. 
        /// </summary>
        /// <param name="timeNowParam"></param>
        /// <returns></returns>
        ParityOfTheWeekModel GenerateDataOfTheWeek(DateTime timeNowParam);

        /// <summary>
        /// Возвращает количество прошедших недель с сентября до <see cref="DateTime"/> переданного параметра.
        /// </summary>
        /// <param name="timeNowParam"></param>
        /// <returns></returns>
        int GetCountParityOfWeek(DateTime timeNowParam);

        /// <summary>
        /// Возвращает неделю года, к которой относится дата в заданном значении <see cref="DateTime"/> параметра.
        /// </summary>
        /// <param name="timeNowParam"></param>
        /// <returns></returns>
        int GetWeekNumberOfYear(DateTime timeNowParam);

        /// <summary>
        /// Возвращает номер части курса. 
        /// </summary>
        /// <param name="timeNowParam"></param>
        /// <returns></returns>
        int GetPartOfSemester(DateTime timeNowParam);

        /// <summary>
        /// Возвращает количество прошедних семестров в зависимости от <see cref="DateTime"/> параметра 
        /// и начала года обучения (значение находится в конфигурационном файле).
        /// </summary>
        /// <param name="timeNowParam"></param>
        /// <param name="startLearningYear"></param>
        /// <returns></returns>
        int GetNumberOfSemester(DateTime timeNowParam, int startLearningYear);

        /// <summary>
        /// Метод для маппинга DTO модели к ViewModel.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        ParityOfTheWeekViewModel PrepareParityOfTheWeekViewModel(ParityOfTheWeekModel input);
    }
}
