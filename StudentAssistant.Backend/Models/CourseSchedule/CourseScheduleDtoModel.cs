﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAssistant.Backend.Models.CourseSchedule
{
    /// <summary>
    /// Dto модель с данными для получения расписания.
    /// </summary>
    public class CourseScheduleDtoModel
    {
        /// <summary>
        /// Время, для которого нужно вернуть расписание.
        /// </summary>
        public DateTimeOffset DateTimeRequest { get; set; }
    }
}
