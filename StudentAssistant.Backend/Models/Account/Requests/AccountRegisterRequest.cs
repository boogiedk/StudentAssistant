using System.ComponentModel.DataAnnotations;

namespace StudentAssistant.Backend.Models.Account.Requests
{
    public class AccountRegisterRequest
    {
        [Required]
        public string Login { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        [Required]
        public string GroupName { get; set; }
    }
}