using model;

namespace persistence.interfaces;

public interface ISoftUserRepository : IRepository<long, SoftUser>
{
    public SoftUser FindByUsernameAndPassword(string username, string password);
}