using System;
using AutoMapper;
using Microsoft.Extensions.Options;
using StudentAssistant.Backend.Models.Email;
using StudentAssistant.Backend.Models.UserSupport;

namespace StudentAssistant.Backend.Services.Implementation
{
    public class UserSupportService : IUserSupportService
    {
        private readonly EmailServiceConfigurationModel _emailServiceConfigurationModel;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public UserSupportService(
            IOptions<EmailServiceConfigurationModel> emailServiceConfigurationModel,
            IEmailService emailService,
            IMapper mapper
        )
        {
            _emailServiceConfigurationModel = emailServiceConfigurationModel.Value ?? throw new ArgumentNullException(nameof(emailServiceConfigurationModel));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public UserFeedbackResultModel SendFeedback(UserFeedbackRequestModel input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            // подготовка модели для отправки фидбека через почтовый сервис
            var emailRequestModel = PrepareUserFeedbackRequestForEmailService(input);

            // отправка фидбека через почтовый сервис
            var resultEmailModel = _emailService.Send(emailRequestModel);

            var feedbackResultModel = _mapper.Map<UserFeedbackResultModel>(resultEmailModel);

            return feedbackResultModel;
        }

        /// <summary>
        /// Метод для подготовки и маппинга модели <see cref="UserFeedbackRequestModel"/> в <see cref="EmailRequestModel"/>.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public EmailRequestModel PrepareUserFeedbackRequestForEmailService(UserFeedbackRequestModel input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            var emailRequestModel = _mapper.Map<EmailRequestModel>(input);

            // из файла с конфигом достаем настройки аккаунта
            emailRequestModel.EmailAccount = new EmailAccountModel
            {
                EmailFrom = _emailServiceConfigurationModel.EmailFrom,
                Id = _emailServiceConfigurationModel.Id,
                HiddenEmail = _emailServiceConfigurationModel.HiddenEmail,
                InputEnableSSL = _emailServiceConfigurationModel.InputEnableSSL,
                InputHost = _emailServiceConfigurationModel.InputHost,
                Login = _emailServiceConfigurationModel.Login,
                OutputPort = _emailServiceConfigurationModel.OutputPort,
                Password = _emailServiceConfigurationModel.Password,
                UseDefaultCredentials = _emailServiceConfigurationModel.UseDefaultCredentials,
                OutputEnableSSL = _emailServiceConfigurationModel.OutputEnableSSL,
                OutputHost = _emailServiceConfigurationModel.OutputHost,
                InputPort = _emailServiceConfigurationModel.InputPort
            };

            return emailRequestModel;
        }
    }
}
