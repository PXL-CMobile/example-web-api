using AutoMapper;
using ExampleWebApi.Api.Models;
using ExampleWebApi.Domain;
using ExampleWebApi.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Security.Claims;

namespace ExampleWebApi.Api.Controllers
{
    /// <summary>
    /// Controller to Add actors
    /// </summary>
    [Route("api/actors")]
    [Produces("application/json")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly ExampleDbContext _context;
        private readonly IMapper _mapper;
        public ActorsController(ExampleDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets a list of ALL actors, no login needed
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<ActorDTO>> Get()
        {
            return Ok(_mapper.Map<IEnumerable<ActorDTO>>(_context.Actors.ToList()));
        }

        /// <summary>
        /// Gets the actor with a specific Id
        /// </summary>
        /// <param name="id">The id of the actor to GET.</param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public ActionResult<ActorDTO> GetById(int id)
        {
            Actor? actor = _context.Actors.Find(id);
            if (actor is not null)
            {
                return Ok(_mapper.Map<ActorDTO>(actor));
            }
            return NotFound();
        }

        /// <summary>
        /// Gets a list of the favorite actors of the current user, login needed.
        /// </summary>
        [HttpGet("my-actors")]
        public ActionResult<IEnumerable<ActorDTO>> GetMyActors()
        {
            Guid loggedinUser = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var list = _context.Users
                .Where(u => u.Id == loggedinUser)
                .Include(u => u.FavoriteActors)
                .ThenInclude(fa=>fa.Actor)
                .Select(r => r.FavoriteActors)
                .FirstOrDefault();

            var actorList = list?.Select(favo => favo.Actor).ToList();
            return _mapper.Map<List<ActorDTO>>(actorList);
        }

        /// <summary>
        /// Adds an actor to the favorites of a user
        /// </summary>
        /// <param name="actorId">Id of the actor to add to the favorites</param>
        [HttpPost("add-favorite/{actorId:int}")]
        public ActionResult Post(int actorId)
        {
            Guid loggedinUser = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            try
            {
                _context.UserFavoriteActors.Add(new UserFavoriteActor { UserId = loggedinUser, ActorId = actorId });
                _context.SaveChanges();
            } catch(Exception ex)
            {
                // Something went wrong -> Does the user already have this actor as a favorite? 
                return BadRequest("Error while adding favorite.");
            }
            return Ok();
        }

        /// <summary>
        /// Add an Actor, login needed
        /// Uses an ActorModel (an object known to the mapper)
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ActorDTO), StatusCodes.Status201Created)]
        public ActionResult<ActorDTO> Post([FromBody] NewActorModel actorModel)
        {
            Actor newActor = _mapper.Map<Actor>(actorModel);
            _context.Add(newActor);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = newActor.Id }, _mapper.Map<ActorDTO>(newActor));
        }
    }
}
