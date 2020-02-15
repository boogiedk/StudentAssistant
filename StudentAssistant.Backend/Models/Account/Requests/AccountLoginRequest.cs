using System.ComponentModel.DataAnnotations;

namespace StudentAssistant.Backend.Models.Account.Requests
{
    /// <summary>
    /// Модель для авторизации.
    /// </summary>
    public class AccountLoginRequest
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
    }
}