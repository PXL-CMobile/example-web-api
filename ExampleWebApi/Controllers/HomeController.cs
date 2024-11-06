using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace ExampleWebApi.Api.Controllers
{
    [Route("")]
    public class HomeController: Controller
    {
        [HttpGet("")]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return RedirectPermanent("~/swagger");
        }

        [HttpGet("test")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public IActionResult Test()
        {
            return Ok("GET request without authorization working fine");
        }

        [HttpGet("testauth")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public IActionResult TestAuth()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok("GET request WITH authorization working fine");
        }
    }
}
