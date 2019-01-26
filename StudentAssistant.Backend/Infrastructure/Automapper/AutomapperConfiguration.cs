using System.Globalization;
using AutoMapper;
using StudentAssistant.Backend.Models;
using StudentAssistant.Backend.Models.Email;
using StudentAssistant.Backend.Models.UserSupport;
using StudentAssistant.Backend.Models.ViewModels;

namespace StudentAssistant.Backend.Infrastructure.AutoMapper
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<ParityOfTheWeekModel, ParityOfTheWeekViewModel>()
                .ForMember(destination => destination.DateTimeRequest,
                    opts => opts.MapFrom(src => src.DateTimeRequest.ToString(CultureInfo.InvariantCulture)));

            CreateMap<UserFeedbackRequestModel, EmailRequestModel>();

            CreateMap<EmailResultModel, UserSupportResultModel>();

            //.ForMember(dest => dest.BookTitle,
            //    opts => opts.MapFrom(src => src.Title));

        }
    }
}
