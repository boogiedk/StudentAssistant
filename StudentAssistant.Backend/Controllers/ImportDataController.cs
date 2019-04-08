using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAssistant.Backend.Services;
using StudentAssistant.DbLayer.Services;

namespace StudentAssistant.Backend.Controllers
{
    [Produces("application/json")]
    [Route("api/importData")]
    public class ImportDataController : ControllerBase
    {
        public readonly IImportDataExcelService _iImportDataExcelService;

        public ImportDataController(IImportDataExcelService importDataExcelService)
        {
            _iImportDataExcelService = importDataExcelService;
        }

        [Route("loadfile")]
        [HttpGet]
        public IActionResult LoadExcelFile()
        {
            try
            {
                var listImportDataExcelModel = _iImportDataExcelService.LoadExcelFile();

                return Ok();
            }
            catch (Exception ex)
            {
               return BadRequest(ex);
            }
        }
    }
}