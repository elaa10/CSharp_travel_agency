using model;

namespace persistence.interfaces;

public interface ISoftUserRepository : IRepository<long, SoftUser>
{
    SoftUser FindByUsernameAndPassword(string username, string password);
}