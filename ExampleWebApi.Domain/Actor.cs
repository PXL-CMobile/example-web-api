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
        public Guid UserId { get; set; }
    }
}
