using System.Globalization;
using AutoMapper;
using Humanizer;
using StudentAssistant.Backend.Models.CourseSchedule;
using StudentAssistant.Backend.Models.CourseSchedule.ViewModels;
using StudentAssistant.Backend.Models.Email;
using StudentAssistant.Backend.Models.LogProvider;
using StudentAssistant.Backend.Models.ParityOfTheWeek;
using StudentAssistant.Backend.Models.ParityOfTheWeek.ViewModels;
using StudentAssistant.Backend.Models.UserSupport;
using StudentAssistant.DbLayer.Models.CourseSchedule;

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

            CreateMap<CourseScheduleModel, CourseScheduleDatabaseModel>();

            CreateMap<CourseScheduleDatabaseModel, CourseScheduleModel>();

            CreateMap<CourseScheduleModel, CourseViewModel>()
                .ForMember(destination => destination.CourseType, opts => opts.MapFrom(src => src.CourseType.Humanize()))
                .ForMember(destination => destination.ParityWeek, opts => opts.MapFrom(src => src.ParityWeek ? "чётной" : "нечётной"))
                .ForMember(destination => destination.NumberWeek, opts => opts.MapFrom(src => string.Join(", ", src.NumberWeek)))
                .ForMember(destination => destination.CombinedGroup, opts => opts.MapFrom(src => string.Join(", ", src.CombinedGroup)));
            //.ForMember(dest => dest.BookTitle,
            //    opts => opts.MapFrom(src => src.Title));

            CreateMap<LogDtoResponseModel, LogResponseModel>();

        }
    }
}
