using System;
using Microsoft.AspNetCore.Mvc;

namespace sample.microservices.webapi.Controllers
{
    [ApiController]
    public class HelloWorldController : ControllerBase
    {
        [HttpGet("hello")]
        public ActionResult<string> Get()
        {
            Console.WriteLine("Hello, World from API 2.");
            return "Hello, World from API 2";
        }
    }
}
