using System.ComponentModel.DataAnnotations;

namespace StudentAssistant.Backend.Models.LogProvider
{
    public class LogRequestModel
    {
        [Required] public string LogType { get; set; }
    }
}