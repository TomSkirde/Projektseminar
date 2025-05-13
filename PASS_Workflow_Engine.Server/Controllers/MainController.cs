using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using PASS_Workflow_Engine.Server.HelperClasses;
using TestUploadAPI.Records;

namespace PASS_Workflow_Engine.Server.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MainController : ControllerBase
    {

        [HttpPost]
        public IActionResult UploadFile(IFormFile owlfile)
        {
            return Ok(new UploadHandler().Upload(owlfile));
        }

        [HttpGet]
        public IActionResult GetJsons()
        {
            return Ok(new UploadFolderHandler().GetJsonsOfModels());
        }
        /*
        [HttpPost]
        public IActionResult LoadModel([FromBody] OwlFile file)
        {
            return Ok(new AlpsApiHandler().LoadOwl(file));
        }

        */
    }
}

