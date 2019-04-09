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
        public ImportDataController()
        {
        }

    }
}