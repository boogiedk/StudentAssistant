using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using StudentAssistant.Backend.Models;
using StudentAssistant.Backend.Models.UserSupport;
using StudentAssistant.Backend.Models.ViewModels;
using StudentAssistant.Backend.Services;

namespace StudentAssistant.Backend.Infrastructure.Automapper
{
    public class AutomapperConfiguration : Profile
    {
        public AutomapperConfiguration()
        {
            CreateMap<ParityOfTheWeekModel, ParityOfTheWeekViewModel>()
                .ForMember(destination => destination.DateTimeRequest,
                    opts => opts.MapFrom(src => src.DateTimeRequest.ToString(CultureInfo.InvariantCulture)));

            CreateMap<UserFeedbackRequestModel, EmailRequestModel>();

            //.ForMember(dest => dest.BookTitle,
            //    opts => opts.MapFrom(src => src.Title));

        }
    }
}
