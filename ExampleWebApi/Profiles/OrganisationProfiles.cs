using AutoMapper;
using ExampleWebApi.Api.Models;
using ExampleWebApi.Domain;

namespace ExampleWebApi.Api.Profiles
{
    public class OrganisationProfiles : Profile
    {
        public OrganisationProfiles()
        {
            CreateMap<Actor, ActorDTO>().ReverseMap();
        }
    }
}
