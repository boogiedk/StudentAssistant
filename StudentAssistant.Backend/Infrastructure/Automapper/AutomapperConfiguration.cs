using System.Globalization;
using AutoMapper;
using Humanizer;
using StudentAssistant.Backend.Models;
using StudentAssistant.Backend.Models.CourseSchedule;
using StudentAssistant.Backend.Models.Email;
using StudentAssistant.Backend.Models.ParityOfTheWeek;
using StudentAssistant.Backend.Models.ParityOfTheWeek.ViewModels;
using StudentAssistant.Backend.Models.UserSupport;
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

            CreateMap<CourseScheduleResultModel, CourseViewModel>()
                .ForMember(destination => destination.CourseType, opts => opts.MapFrom(src => src.CourseType.Humanize()))
                .ForMember(destination => destination.ParityWeek, opts => opts.MapFrom(src => src.ParityWeek ? "Чётная" : "Нечётная"))
                .ForMember(destination => destination.NumberWeek, opts => opts.MapFrom(src => string.Join(", ", src.NumberWeek)));
            //.ForMember(dest => dest.BookTitle,
            //    opts => opts.MapFrom(src => src.Title));

        }
    }
}
