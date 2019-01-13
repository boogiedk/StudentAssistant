using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using StudentAssistant.Backend.Models.UserSupport;

namespace StudentAssistant.Backend.Services.Implementation
{
    public class UserSupportService : IUserSupportService
    {
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public UserSupportService(IMapper mapper,IEmailService emailService)
        {
            _mapper = mapper;
            _emailService = emailService;
        }

        public UserFeedbackResultModel SendFeedback(UserFeedbackRequestModel input)
        {
            if (input == null) throw new NotSupportedException($"{typeof(UserFeedbackRequestModel)} input равен null");

            var emailRequestModel = _mapper.Map<EmailRequestModel>(input);

            var resultEmailModel = _emailService.SendEmail(emailRequestModel);

            var feedbackResultModel = _mapper.Map<UserFeedbackResultModel>(resultEmailModel);

            return feedbackResultModel;
        }
    }
}
