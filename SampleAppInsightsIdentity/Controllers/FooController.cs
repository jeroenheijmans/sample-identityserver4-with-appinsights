using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SampleAppInsightsIdentity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FooController : ControllerBase
    {
        [HttpGet]
        public Response Get() => new Response { Data = "Unprotected endpoint. Go GET /protected to try auth" };

        [Authorize]
        [HttpGet("protected")]
        public Response GetProtected() => new Response { Data = "Unprotected endpoint. Go to /protected to try auth" };
    }

    public class Response
    {
        public DateTime Timestamp => DateTime.Now;
        public string Data { get; set; } = "Default data";
    }
}
