namespace StudentAssistant.Backend.Services
{
    /// <summary>
    /// Модель, содержащая данные об аккаунте отправителя.
    /// </summary>
    public class EmailAccountModel
    {
        /// <summary>
        /// Идентификатор аккаунта.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Почта, откуда отправлять сообщение.
        /// </summary>
        public string EmailFrom { get; set; }

        /// <summary>
        /// Адрес входящей почты.
        /// </summary>
        public string InputHost { get; set; }

        /// <summary>
        /// Порт для отправки сообщений входящей почты.
        /// </summary>
        public int InputPort { get; set; }

        /// <summary>
        /// Активировано ли SSL подлючение для входящей почты.
        /// 0 - false, 1 - true.
        /// </summary>
        public TypeEncrypt InputEnableSSL { get; set; }

        /// <summary>
        /// Адрес исходящей почты.
        /// </summary>
        public string OutputHost { get; set; }

        /// <summary>
        /// Порт для отправки сообщений исходящей почты.
        /// </summary>
        public int OutputPort { get; set; }

        /// <summary>
        /// Активировано ли SSL подлючение для входящей почты.
        /// 0 - false, 1 - true.
        /// </summary>
        public TypeEncrypt OutputEnableSSL { get; set; }

        /// <summary>
        /// Пароль от почты.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Логин для авторизации.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Почта, на которую отправляются копии сообщений.
        /// </summary>
        public string HiddenEmail { get; set; }

        /// <summary>
        /// Возвращает boolean значение в завимости от того, выбраны ли полномочия по умолчанию.
        /// 0 - false, 1 - true.
        /// </summary>
        public byte UseDefaultCredentials { get; set; }
    }
}