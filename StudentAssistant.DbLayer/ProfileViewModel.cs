using Microsoft.AspNetCore.Identity;
using StudentAssistant.DbLayer.Models;

namespace StudentAssistant.DbLayer
{
    /// <summary>
    /// Модель отображения профиля пользователя.
    /// </summary>
    public class ProfileViewModel
    {
        /// <summary>
        /// Тип пользователя.
        /// </summary>
        public IdentityRoles IdentityRole { get; set; }
        
        /// <summary>
        /// Профиль пользователя. (Временно)
        /// </summary>
        public object ProfileInfo { get; set; }
        
    }
}