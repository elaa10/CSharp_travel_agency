namespace DefaultNamespace;

public class Entity<T>
{
    private T id;

    public T Id
    {
        get { return id; }
        set { id = value; }
    }
}
