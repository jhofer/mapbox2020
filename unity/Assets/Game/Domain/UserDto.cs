
namespace Assets.Game.Domain
{
    public class UserDto
    {

        public string id { get; set; }

        public string name { get; set; }

        public string email { get; set; }

        public bool Equals(UserDto other)
        {
            return id.Equals(other.id) && email.Equals(other.email);
        }
    }
}

