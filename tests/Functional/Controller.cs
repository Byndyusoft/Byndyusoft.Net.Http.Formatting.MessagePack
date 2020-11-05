namespace System.Net.Http.Functional
{
    using Microsoft.AspNetCore.Mvc;
    using Models;

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