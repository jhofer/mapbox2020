public class User
{
   
    public string id { get; set; }

    public string name { get; set; }
 
    public string email { get; set; }

    public bool Equals(User other)
    {
        return id.Equals(other.id) && email.Equals(other.email);
    }
}

