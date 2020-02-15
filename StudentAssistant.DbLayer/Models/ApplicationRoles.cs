using System.ComponentModel;

namespace StudentAssistant.DbLayer.Models
{
    public enum IdentityRoles
    {
        [Description("Administrator")]
        Administrator = 1,
        
        [Description("Student")]
        Student = 2,
        
        [Description("Teacher")]
        Teacher = 3,
        
        [Description("User")]
        User = 0
    }
}