using Microsoft.AspNetCore.Identity;

namespace StudentAssistant.DbLayer
{
    public class ProfileViewModel
    {
        public IdentityRole IdentityRole { get; set; }
        public IProfileInfo ProfileInfo { get; set; }
    }

    public interface IProfileInfo
    {
        
    }
}