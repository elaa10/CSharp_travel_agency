namespace DefaultNamespace;

public class SoftUser : Entity<long>
{
    public string Username { get; set; }
    public string Password { get; set; }

    public SoftUser(string username, string password)
    {
        Username = username;
        Password = password;
    }
}
