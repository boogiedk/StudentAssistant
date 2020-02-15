using System.ComponentModel.DataAnnotations;
using StudentAssistant.DbLayer.Models;

namespace StudentAssistant.Backend.Models.Account.Requests
{
    /// <summary>
    /// Модель регистрации пользователя.
    /// </summary>
    public class AccountRegisterRequest
    {
        /// <summary>
        /// Логин.
        /// </summary>
        [Required]
        public string Login { get; set; }
        
        /// <summary>
        /// Пароль.
        /// </summary>
        [Required]
        public string Password { get; set; }
        
        /// <summary>
        /// Имя.
        /// </summary>
        [Required]
        public string FirstName { get; set; }
        
        /// <summary>
        /// Фамилия.
        /// </summary>
        [Required]
        public string LastName { get; set; }
        
        /// <summary>
        /// Группа.
        /// </summary>
        [Required]
        public string GroupName { get; set; }
        
        /// <summary>
        /// Почта
        /// </summary>
        [Required]
        public string Email { get; set; }
        
        /// <summary>
        /// Роль.
        /// </summary>
        [Required]
        public IdentityRoles ApplicationRoles { get; set; }
    }
}