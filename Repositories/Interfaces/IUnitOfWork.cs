namespace Repositories.Interfaces;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}
