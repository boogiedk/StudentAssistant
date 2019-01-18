﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StudentAssistant.Backend.Models.Email;

namespace StudentAssistant.Backend.Services.Implementation
{
    public class EmailService : IEmailService
    {
        public EmailResultModel SendEmail(EmailRequestModel input)
        {
            try
            {
                if (input?.EmailAccount == null)
                {
                    //log
                    return new EmailResultModel
                    {
                        IsSended = false,
                        Message = "Произошла ошибка при отправке сообщения: отсутствуют входные параметры.",
                    };
                }

                MailMessage messageEmail = new MailMessage(input.EmailTo, input.EmailAccount.EmailFrom);

                messageEmail.Subject = input.Subject; // Заголовок (текст, который появляется в push-уведомлениях
                messageEmail.IsBodyHtml = false;


                if (!string.IsNullOrEmpty(input.EmailAccount.HiddenEmail))
                {
                    messageEmail.Bcc.Add(
                        new MailAddress(input.EmailAccount
                            .HiddenEmail)); // hiddenEmail добавляет адресат, куда отправлять копию отправленного сообщения.
                }

                using (var smtp = new SmtpClient())
                {
                    smtp.Port = input.EmailAccount.OutputPort;
                    smtp.UseDefaultCredentials = input.EmailAccount.UseDefaultCredentials == 1 ? true : false;
                    smtp.Host = input.EmailAccount.OutputHost;
                    smtp.EnableSsl = input.EmailAccount.OutputEnableSSL == TypeEncrypt.SSL;
                    smtp.Credentials = new NetworkCredential(input.EmailAccount.Login, input.EmailAccount.Password);

                    smtp.Send(messageEmail);
                }

                return new EmailResultModel {IsSended = true};
            }
            catch (Exception ex)
            {
                //log
                return new EmailResultModel
                {
                    IsSended = false,
                    Message = "Произошла ошибка при отправке сообщения: возникло исключение в системе."
                };
            }
        }
    }
}
