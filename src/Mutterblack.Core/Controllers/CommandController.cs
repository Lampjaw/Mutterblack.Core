using Microsoft.AspNetCore.Mvc;
using Mutterblack.Core.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mutterblack.Core.Controllers
{
    [Route("command")]
    public class CommandController : Controller
    {
        private readonly CommandHelper _commandHelper;

        public CommandController(CommandHelper commandHelper)
        {
            _commandHelper = commandHelper;
        }

        [HttpPost("{commandGroupName}/{actionName}")]
        public async Task<ActionResult> ExecuteCommand(string commandGroupName, string actionName, [FromBody]Dictionary<string, object> args)
        {
            var result = await _commandHelper.ExecuteCommand(commandGroupName, actionName, args);
            return Ok(result);
        }
    }
}
