using Microsoft.Azure.Cosmos.Spatial;

namespace Endgame.Backend.Domain
{
    public class User
    {

        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public bool Equals(User other)
        {
            return Id.Equals(other.Id) && Email.Equals(other.Email);
        }

        public Point Location { get; set; }
    }
}

