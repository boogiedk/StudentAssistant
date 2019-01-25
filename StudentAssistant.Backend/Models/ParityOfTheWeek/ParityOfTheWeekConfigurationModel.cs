namespace StudentAssistant.Backend.Models.ConfigurationModels
{
    /// <summary>
    /// Модель с данными для сервиса, генерирующего данные о дне.
    /// </summary>
    public class ParityOfTheWeekConfigurationModel
    {
        public int StartLearningYear { get; set; }

        public static ParityOfTheWeekConfigurationModel GetDefaulfValues()
        {
            var result = new ParityOfTheWeekConfigurationModel
            {
                StartLearningYear = 2016
            };

            return result;
        }
    }
}