using Microsoft.AspNetCore.Mvc;
using yomo.Models;

namespace yomo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class yomoApiController : ControllerBase
    {
        public class yomoApiRecord
        {
            public Monitor Monitor;
            public Configuration Configuration;
            public GeometryCatelogEntry[] GeometryCatelog;
        }

        // GET: YoMoOps monitor and configuration settings
        [HttpGet]
        public yomoApiRecord Get()
        {
            // Update the monitor info and pull the configuration from disk

            return new yomoApiRecord()
            {
                Monitor = new Monitor(),
                Configuration = new Configuration(),
                GeometryCatelog = new GeometryCatelogEntry[] {
                    new GeometryCatelogEntry(){Id="rgn1", Name="nm1", Shape=Shape.Region },
                    new GeometryCatelogEntry(){Id="rgn2", Name="nm2",Shape=Shape.Region },
                    new GeometryCatelogEntry(){Id="rtA", Name="nmA", Shape=Shape.Route }
                }
            };
        }

        /// <summary>
        /// Execute the command provided
        /// </summary>
        /// <param name="command"></param>
        [HttpPost]
        public void Post([FromBody] yomo.Models.Command command)
        {
            // Execute the command provided
        }

        /// <summary>
        /// Update the configuration
        /// </summary>
        /// <param name="configuration"></param>
        [HttpPut]
        public void Put([FromBody] Configuration configuration)
        {
        }
    }
}
