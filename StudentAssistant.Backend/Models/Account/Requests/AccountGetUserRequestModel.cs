using System.ComponentModel.DataAnnotations;

namespace StudentAssistant.Backend.Models.Account.Requests
{
    /// <summary>
    /// Модель для запроса аккаунта пользователя.
    /// </summary>
    public class AccountGetRequestModel
    {
        /// <summary>
        /// Логин.
        /// </summary>
        [Required]
        public string Login { get; set; }
    }
}