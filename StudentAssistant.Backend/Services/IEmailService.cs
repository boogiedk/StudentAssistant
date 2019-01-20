﻿using StudentAssistant.Backend.Models.Email;

namespace StudentAssistant.Backend.Services
{
    /// <summary>
    /// E-mail сервис.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Отправляет E-mail сообщение
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        EmailResultModel SendEmail(EmailRequestModel input);
    }
}