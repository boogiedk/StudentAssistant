using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace StudentAssistant.DbLayer.Models
{
    /// <summary>
    /// Преподаватель.
    /// </summary>
    public class TeacherModel : IProfileInfo
    {
        /// <summary>
        /// Идентификатор пользователя системы.
        /// </summary>
        public Guid Id { get; set; }
        
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
        
        /// <summary>
        /// Пользователь приложения.
        /// </summary>
        public IdentityUser IdentityUser { get; set; }
    }
}