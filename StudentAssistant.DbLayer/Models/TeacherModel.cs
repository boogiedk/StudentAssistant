using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAssistant.DbLayer.Models
{
    /// <summary>
    /// Преподаватель.
    /// </summary>
    public class TeacherModel : UserModel
    {
        /// <summary>
        /// Имя.
        /// </summary>
        public string FirstName { get; set; }
        
        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }
        
        /// <summary>
        /// Отчество.
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Полное имя (временно)
        /// </summary>
        public string FullName { get; set; }
    }
}