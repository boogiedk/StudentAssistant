using System.Globalization;
using AutoMapper;
using Humanizer;
using StudentAssistant.Backend.Models;
using StudentAssistant.Backend.Models.Email;
using StudentAssistant.Backend.Models.UserSupport;
using StudentAssistant.Backend.Models.ViewModels;
using StudentAssistant.Backend.Services;
using StudentAssistant.Backend.Services.Implementation;
using StudentAssistant.DbLayer.Services;

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

            CreateMap<CourseScheduleResultModel, CourseScheduleDatabaseModel>();

            CreateMap<CourseScheduleResultModel, CourseScheduleViewModel>()
                .ForMember(destination => destination.CourseType, opts => opts.MapFrom(src => src.CourseType.Humanize()));

            //.ForMember(dest => dest.BookTitle,
            //    opts => opts.MapFrom(src => src.Title));

        }
    }
}
