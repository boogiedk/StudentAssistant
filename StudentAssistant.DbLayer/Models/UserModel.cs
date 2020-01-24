using System;

namespace StudentAssistant.DbLayer.Models
{
    /// <summary>
    /// Пользователь сервиса.
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Логин пользователя.
        /// </summary>
        public string Login { get; set; }
        
        /// <summary>
        /// Хеш пароля пользователя.
        /// </summary>
        public string PasswordHash { get; set; }
    }
}