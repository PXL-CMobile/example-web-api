using AutoMapper;
using ExampleWebApi.Api.Models;
using ExampleWebApi.Domain;
using ExampleWebApi.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Security.Claims;

namespace ExampleWebApi.Api.Controllers
{
    /// <summary>
    /// Controller to Add actors
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ActorController : ControllerBase
    {
        private readonly ExampleDbContext _context;
        private readonly IMapper _mapper;
        public ActorController(ExampleDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets a list of ALL actors, no login needed
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<Actor> Get()
        {
            return _context.Actors.ToList();
        }

        /// <summary>
        /// Gets a list of actors of the current user, login needed
        /// </summary>
        [HttpGet("my-actors")]
        public IEnumerable<Actor> GetMyActors()
        {
            Guid loggedinUser = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return _context.Actors.Where(a => a.UserId == loggedinUser).ToList();
        }

        /// <summary>
        /// Add an Actor, login needed
        /// </summary>
        [HttpPost]
        public void Post([FromBody] ActorDTO actorDTO)
        {
            Actor newActor = _mapper.Map<Actor>(actorDTO);
            newActor.UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _context.Add(newActor);
            _context.SaveChanges();
        }
    }
}
