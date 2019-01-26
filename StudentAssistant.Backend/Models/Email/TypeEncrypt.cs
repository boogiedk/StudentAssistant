namespace StudentAssistant.Backend.Services
{
    /// <summary>
    /// Тип шифрования.
    /// </summary>
    public enum TypeEncrypt
    {
        /// <summary>
        /// SSL шифрование.
        /// </summary>
        SSL = 1,
        /// <summary>
        /// TLS шифрование.
        /// </summary>
        TLS = 2,
        /// <summary>
        /// Отсутствует шифрование.
        /// </summary>
        Nothing = 0
    }
}