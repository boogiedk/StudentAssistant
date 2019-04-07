using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAssistant.Backend.Services;

namespace StudentAssistant.Backend.Controllers
{
    [Produces("application/json")]
    [Route("api/importData")]
    public class ImportDataController : ControllerBase
    {
        public readonly IImportDataExcelService _importDataExcelService;

        public ImportDataController(IImportDataExcelService importDataExcelService)
        {
            _importDataExcelService = importDataExcelService;
        }

        [Route("loadfile")]
        [HttpGet]
        public IActionResult LoadExcelFile()
        {
           _importDataExcelService.LoadExcelFile();

           return Ok();
        }
    }
}