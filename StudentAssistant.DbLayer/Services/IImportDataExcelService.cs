using System.Collections.Generic;
using StudentAssistant.DbLayer.Models.ImportData;

namespace StudentAssistant.DbLayer.Services
{
    /// <summary>
    /// Сервис для импорта данных из Excel.
    /// </summary>
    public interface IImportDataExcelService
    {
        /// <summary>
        /// Импортирует данные из Excel файла.
        /// </summary>
        List<ImportDataExcelModel> LoadExcelFile();

        /// <summary>
        /// Маппит модель из модели иморта в модель в модель базы данных.
        /// </summary>
        /// <param name="importDataExcelModels"></param>
        /// <returns></returns>
        List<CourseScheduleDatabaseModel> PrepareImportDataExcelModelToDatabaseModel(
            List<ImportDataExcelModel> importDataExcelModels);
    }
}
