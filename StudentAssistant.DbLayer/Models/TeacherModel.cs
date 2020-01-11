using System;

namespace StudentAssistant.DbLayer.Models
{
    public class TeacherModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        
        public string FullName { get; set; }
    }
}