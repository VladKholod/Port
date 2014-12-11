using System.Collections.Generic;
using System.Threading.Tasks;
using PortSimulator.Core.Entities;

namespace PortSimulator.DatabaseManager.Repositories.RepositoryAbstractions
{
    public interface IRepository<T> where T : Entity
    {
        T GetEntity(int id);
        Task Save(T entity);
        Task Delete(int id);
        List<T> GetAll();
    }
}
