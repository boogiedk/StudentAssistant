﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAssistant.Backend.Models.CourseSchedule
{
    public class CourseScheduleUpdateByLinkAsyncModel
    {
        /// <summary>
        /// Ссылка на скачивание.
        /// </summary>
        [Required]
        public Uri Uri { get; set; }
    }
}
