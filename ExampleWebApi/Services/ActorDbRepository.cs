using ExampleWebApi.Api.Services.Contracts;
using ExampleWebApi.Domain;
using ExampleWebApi.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ExampleWebApi.Api.Services
{
    public class ActorDbRepository : IActorRepository
    {
        private readonly ExampleDbContext _context;

        public ActorDbRepository(ExampleDbContext context) 
        {
            _context = context;
        }

        public int AddActor(Actor newActor)
        {
            _context.Add(newActor);
            _context.SaveChanges();
            return newActor.Id;
        }

        public void AddFavoriteActor(Guid loggedinUser, int actorId)
        {
            _context.UserFavoriteActors.Add(new UserFavoriteActor { UserId = loggedinUser, ActorId = actorId });
            _context.SaveChanges();
        }

        public Actor? FindActorById(int id)
        {
            return _context.Actors.Find(id);
        }

        public IEnumerable<Actor> GetAllActors()
        {
            return _context.Actors.ToList();
        }

        public IEnumerable<Actor>? GetFavoriteActorsUser(Guid loggedinUser)
        {
            var list = _context.Users
                .Where(u => u.Id == loggedinUser)
                .Include(u => u.FavoriteActors)
                .ThenInclude(fa => fa.Actor)
                .Select(r => r.FavoriteActors)
                .FirstOrDefault();

            var actorList = list?.Select(favo => favo.Actor).ToList();

            return actorList;
        }
    }
}
