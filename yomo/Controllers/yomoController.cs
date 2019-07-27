using Microsoft.AspNetCore.Mvc;

namespace yomo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class yomoController : ControllerBase
    {
        [HttpGet]
        [Produces("text/html")]
        public string Get()
        {
            string responseString = @" 
            <title>My report</title>
            <style type='text/css'>
            button{
                color: green;
            }
            </style>
            <h1> Header </h1>
            <p>Hello There <button>click me</button></p>
            <p style='color:blue;'>I am blue</p>
            ";
            return responseString;
        }
    }
}