namespace StudentAssistant.Backend.Models.Account.Responses
{
    public class AccountRegisterResponse
    {
        /// <summary>
        /// Успешность регистрации.
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Токен.
        /// </summary>
        public string Token { get; set; }
        
    }
}