using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using PortSimulator.Core.Entities;

namespace PortSimulator.DatabaseManager.Repositories.RepositoryAbstractions
{
    public abstract class BaseRepository<T> where T : Entity
    {
        protected readonly Dictionary<string, string> Queries = new Dictionary<string, string>();
        
        protected readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["PortDB"].ConnectionString;

        protected abstract T ReadEntity(SqlDataReader reader);

        protected abstract void LoadBaseQueries();
    }
}
