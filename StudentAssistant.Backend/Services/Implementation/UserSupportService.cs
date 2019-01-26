using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using StudentAssistant.Backend.Models.Email;
using StudentAssistant.Backend.Models.UserSupport;

namespace StudentAssistant.Backend.Services.Implementation
{
    public class UserSupportService : IUserSupportService
    {
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly EmailServiceConfigurationModel _config;

        public UserSupportService(IMapper mapper,IEmailService emailService, EmailServiceConfigurationModel config)
        {
            _mapper = mapper;
            _emailService = emailService;
            _config = config;
        }

        public UserFeedbackResultModel SendFeedback(UserFeedbackRequestModel input)
        {
            if (input == null) throw new NotSupportedException($"{typeof(UserFeedbackRequestModel)} input равен null");

            // подготовка модели для отправки фидбека через почтовый сервис
            var emailRequestModel = PrepareUserFeedbackRequestForEmailService(input);

            // отправка фидбека через почтовый сервис
            var resultEmailModel = _emailService.SendEmail(emailRequestModel);

            var feedbackResultModel = _mapper.Map<UserFeedbackResultModel>(resultEmailModel);

            return feedbackResultModel;
        }

        /// <summary>
        /// Метод для подготовки и маппинга модели <see cref="UserFeedbackRequestModel"/> в <see cref="EmailRequestModel"/>.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private EmailRequestModel PrepareUserFeedbackRequestForEmailService(UserFeedbackRequestModel input)
        {
            if (input == null) throw new NotSupportedException($"{typeof(UserFeedbackRequestModel)} input равен null");

            var emailRequestModel = _mapper.Map<EmailRequestModel>(input);

            emailRequestModel.EmailAccount = _config.EmailAccountModel;

            return emailRequestModel;
        }
    }
}
