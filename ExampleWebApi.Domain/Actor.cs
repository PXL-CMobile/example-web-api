using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleWebApi.Domain
{
    public class Actor
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public int BirthYear { get; set; }

        public List<UserFavoriteActor> UserFavorites { get; set; } = new();
    }
}
