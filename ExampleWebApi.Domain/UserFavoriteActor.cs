using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleWebApi.Domain
{
    public class UserFavoriteActor
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public int ActorId { get; set; }
        public Actor Actor { get; set; }
    }
}
