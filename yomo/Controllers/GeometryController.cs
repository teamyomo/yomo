using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using yomo.Models;

namespace yomo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GeometryController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IDictionary<string, string>> Get()
        {
            return new Dictionary<string, string> {
                { "key1", "pretty name1" },
                { "key2", "pretty name2" },
            };
        }
  
        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<Geometry> Get(string id)
        {
            return new Geometry();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] Geometry value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] Geometry value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
        }
    }
}
