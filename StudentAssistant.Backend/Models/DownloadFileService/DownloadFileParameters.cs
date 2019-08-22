using System;

namespace StudentAssistant.Backend.Models.DownloadFileService
{
    /// <summary>
    /// Модель для хранения данных о скачеваемом файле и источнике.
    /// </summary>
    public class DownloadFileParametersModel
    {
        /// <summary>
        /// Имя файла на сайте.
        /// </summary>
        public string FileNameRemote { get; set; }

        /// <summary>
        /// Формат файла.
        /// </summary>
        public string FileFormat { get; set; }

        /// <summary>
        /// Адрес сайта.
        /// </summary>
        public Uri RemoteUri { get; set; }

        /// <summary>
        /// Имя сохраняемого файла.
        /// </summary>
        public string FileNameLocal { get; set; }

        /// <summary>
        /// Путь файла.
        /// </summary>
        public string PathToFile { get; set; }
    }
}
