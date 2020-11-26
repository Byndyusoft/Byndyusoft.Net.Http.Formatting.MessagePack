using System.Net.Http.Formatting.Models;
using Microsoft.AspNetCore.Mvc;

namespace System.Net.Http.Formatting.Functional
{
    [Controller]
    [Route("msgpack-formatter")]
    public class MessagePackFormatterController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] SimpleType model)
        {
            return Ok(model);
        }

        [HttpPut]
        public IActionResult Put([FromBody] SimpleType model)
        {
            return Ok(model);
        }
    }
}
