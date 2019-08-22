using System.ComponentModel.DataAnnotations;

namespace StudentAssistant.Backend.Models.Account.Requests
{
    public class AccountLoginRequest
    {
        [Required]
        public string Login { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}