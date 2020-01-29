using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SampleAppInsightsIdentity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FooController : ControllerBase
    {
        private readonly ILogger<FooController> logger;

        public FooController(ILogger<FooController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public Response Get() => new Response { Data = "Unprotected endpoint. Go GET /protected to try auth" };

        [Authorize]
        [HttpGet("protected")]
        public Response GetProtected() => new Response { Data = "Unprotected endpoint. Go to /protected to try auth" };

        [HttpGet("error")]
        public Response GetError() => throw new InvalidOperationException("Forced an exception in the API for demo purposes");

        [HttpGet("log-warning")]
        public Response LogWarning()
        {
            logger.LogWarning("Forced a warning to be logged");
            return new Response { Data = "Forced a warning to be logged" };
        }

        [HttpGet("log-error")]
        public Response LogError()
        {
            logger.LogError("Forced an error to be logged");
            return new Response { Data = "Forced an error to be logged" };
        }
    }

    public class Response
    {
        public DateTime Timestamp => DateTime.Now;
        public string Data { get; set; } = "Default data";
    }
}
