namespace Galaxon.Astronomy.SpaceCalendars.com.Repositories;

public interface IRepository<T>
{
    public IEnumerable<T> GetAll();

    public T? GetById(int id);

    public void Create(T doc);

    public void Update(T doc);

    public void Delete(int id);
}
