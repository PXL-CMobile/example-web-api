using AutoMapper;
using ExampleWebApi.Api.Models;
using ExampleWebApi.Api.Services.Contracts;
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
    public class ActorsController : ApiControllerBase
    {
        private readonly IActorRepository _repository;
        private readonly IMapper _mapper;

        public ActorsController(IActorRepository repo, IMapper mapper)
        {
            _repository = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets a list of ALL actors, no login needed
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<ActorDTO>> Get()
        {
            // Get all the actors via the context.
            var listOfAllActors = _repository.GetAllActors();
            return Ok(_mapper.Map<IEnumerable<ActorDTO>>(listOfAllActors));
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
            Actor? actor = _repository.FindActorById(id);
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
            // This call is available via the base class. There is a property UserId
            // Guid loggedinUser = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Guid loggedinUser = this.UserId;

            var actorList = _repository.GetFavoriteActorsUser(loggedinUser);
            return _mapper.Map<List<ActorDTO>>(actorList);
        }

        /// <summary>
        /// Adds an actor to the favorites of a user
        /// </summary>
        /// <param name="actorId">Id of the actor to add to the favorites</param>
        [HttpPost("add-favorite/{actorId:int}")]
        public ActionResult Post(int actorId)
        {
            Guid loggedinUser = this.UserId;

            try
            {
                _repository.AddFavoriteActor(loggedinUser, actorId);
            } 
            catch(Exception ex)
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

            int newActorId = _repository.AddActor(newActor);

            return CreatedAtAction(nameof(GetById), new { id = newActorId }, _mapper.Map<ActorDTO>(newActor));
        }
    }
}
