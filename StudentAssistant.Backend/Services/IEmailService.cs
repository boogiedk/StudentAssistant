using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAssistant.Backend.Services
{
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
