using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAssistant.DbLayer.Models.CourseSchedule
{
    public class NumberWeekModel
    {
        public Guid Id { get; set; }
        public int NumberWeek { get; set; }
        
        public override string ToString()
        {
            return NumberWeek.ToString();
        }
    }
}