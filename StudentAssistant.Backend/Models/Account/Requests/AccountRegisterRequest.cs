using System.ComponentModel.DataAnnotations;

namespace StudentAssistant.Backend.Models.Account.Requests
{
    public class AccountRegisterRequest
    {
        [Required]
        public string Login { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}