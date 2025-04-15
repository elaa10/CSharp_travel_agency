using model;

namespace persistence.interfaces;

public interface IRepository<ID, E> where E : Entity<ID>
{
    E FindOne(ID id);
    IEnumerable<E> FindAll();
    bool Save(E entity);
    bool Delete(ID id);
    bool Update(E entity);
}
