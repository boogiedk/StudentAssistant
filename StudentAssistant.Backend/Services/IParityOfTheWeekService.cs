using System;
using StudentAssistant.Backend.Models.ParityOfTheWeek;
using StudentAssistant.Backend.Models.ParityOfTheWeek.ViewModels;

namespace StudentAssistant.Backend.Services
{
    /// <summary>
    /// Интерфейс для работы с сервисом, связанного с формированием данных о заданном <see cref="DateTimeOffset"/> параметре.
    /// </summary>
    public interface IParityOfTheWeekService
    {
        /// <summary>
        /// Возвращает <see cref="bool"/> true, если неделя чётная, иначе <see cref="bool"/> false.
        /// </summary>
        /// <param name="dateTimeOffsetParam"></param>
        /// <returns></returns>  
        bool GetParityOfTheWeekByDateTime(DateTimeOffset dateTimeOffsetParam);

        /// <summary>
        /// Генерирует и возвращает <see cref="ParityOfTheWeekModel"/> модель с данными о текущем дне по заданному <see cref="DateTimeOffset"/>. 
        /// </summary>
        /// <param name="dateTimeOffsetParam"></param>
        /// <returns></returns>
        ParityOfTheWeekModel GenerateDataOfTheWeek(DateTimeOffset dateTimeOffsetParam);

        /// <summary>
        /// Возвращает количество прошедших недель с сентября до <see cref="DateTimeOffset"/> переданного параметра.
        /// </summary>
        /// <param name="dateTimeOffsetParam"></param>
        /// <returns></returns>
        int GetCountParityOfWeek(DateTimeOffset dateTimeOffsetParam);

        /// <summary>
        /// Возвращает неделю года, к которой относится дата в заданном значении <see cref="DateTimeOffset"/> параметра.
        /// </summary>
        /// <param name="dateTimeOffsetParam"></param>
        /// <returns></returns>
        int GetWeekNumberOfYear(DateTimeOffset dateTimeOffsetParam);

        /// <summary>
        /// Возвращает номер части курса. 
        /// </summary>
        /// <param name="dateTimeOffsetParam"></param>
        /// <returns></returns>
        int GetPartOfSemester(DateTimeOffset dateTimeOffsetParam);

        /// <summary>
        /// Возвращает количество прошедних семестров в зависимости от <see cref="DateTimeOffset"/> параметра 
        /// и начала года обучения (значение находится в конфигурационном файле).
        /// </summary>
        /// <param name="dateTimeOffsetParam"></param>
        /// <param name="startLearningYear"></param>
        /// <returns></returns>
        int GetNumberOfSemester(DateTimeOffset dateTimeOffsetParam, int startLearningYear);

        /// <summary>
        /// Метод для маппинга DTO модели к ViewModel.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        ParityOfTheWeekViewModel PrepareViewModel(ParityOfTheWeekModel input);
    }
}
