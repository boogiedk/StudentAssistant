using System;

namespace StudentAssistant.DbLayer.Models
{
    public class StudyGroupModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        public override string ToString()
        {
            return Name;
        }
    }
}