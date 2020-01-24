using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAssistant.DbLayer.Models
{
    /// <summary>
    /// Учебная группа.
    /// </summary>
    public class StudyGroupModel
    {
        public Guid Id { get; set; }
        
        /// <summary>
        /// Имя группы.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Студенты группы.
        /// </summary>
      //  public List<StudentModel> Students { get; set; }
        
        public override string ToString()
        {
            return Name;
        }
    }
}