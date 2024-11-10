using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace ExampleWebApi.Api.Controllers
{
    [Route("")]
    [Produces("application/json")]
    [ApiController]
    public class HomeController: Controller
    {
        [HttpGet("")]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return RedirectPermanent("~/swagger");
        }

        /// <summary>
        /// This endpoint should always work, even when not logged in.
        /// </summary>
        /// <returns></returns>
        [HttpGet("test")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public IActionResult Test()
        {
            return Ok("GET request without authorization working fine");
        }

        /// <summary>
        /// This endpoint is only available if the user is logged in (=> Bearer token set to a valid token).
        /// </summary>
        [HttpGet("testauth")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public IActionResult TestAuth()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var email = User.FindFirst(ClaimTypes.Email).Value;

            return Ok($"GET request WITH authorization working fine, your id is: {userId}, your email is: {email}");
        }
    }
}
