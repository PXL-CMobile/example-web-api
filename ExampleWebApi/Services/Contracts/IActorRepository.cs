using ExampleWebApi.Domain;

namespace ExampleWebApi.Api.Services.Contracts
{
    public interface IActorRepository
    {
        int AddActor(Actor newActor);
        void AddFavoriteActor(Guid loggedinUser, int actorId);
        Actor? FindActorById(int id);
        IEnumerable<Actor> GetAllActors();
        IEnumerable<Actor>? GetFavoriteActorsUser(Guid loggedinUser);
    }
}
