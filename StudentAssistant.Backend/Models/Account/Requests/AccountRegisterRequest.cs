using System.ComponentModel.DataAnnotations;

namespace StudentAssistant.Backend.Models.Account.Requests
{
    public class AccountRegisterRequest
    {
        [Required]
        public string UserName { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
    }
}