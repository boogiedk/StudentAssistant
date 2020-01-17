namespace StudentAssistant.DbLayer.Models
{
    /// <summary>
    /// Студент.
    /// </summary>
    public class StudentModel : UserModel
    {
        /// <summary>
        /// Имя.
        /// </summary>
        public string FirstName { get; set; }
        
        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }
        
        /// <summary>
        /// Отчество.
        /// </summary>
        public string MiddleName { get; set; }
        
        /// <summary>
        /// Учебная группа.
        /// </summary>
        public StudyGroupModel StudyGroupModel { get; set; }
    }
}