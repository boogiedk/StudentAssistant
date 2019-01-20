using StudentAssistant.Backend.Services;

namespace StudentAssistant.Backend.Models.Email
{
    // Модель для конфугурации E-mail сервиса.
    public class EmailServiceConfigurationModel
    {
        public EmailAccountModel EmailAccountModel { get; set; }

        public static EmailServiceConfigurationModel GetDefaultValues()
        {
            var result = new EmailServiceConfigurationModel()
            {
                EmailAccountModel = new EmailAccountModel
                {

                }
            };

            return result;

        }
    }
}