using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

using PortSimulator.Core.Entities;

namespace PortSimulator.DatabaseManager.Repositories.RepositoryAbstractions
{
    public abstract class BaseRepository<T> where T : Entity
    {
        protected Dictionary<string, string> _queries = new Dictionary<string, string>();
        
        protected string _connectionString =
            ConfigurationManager.ConnectionStrings["PortDB"].ConnectionString;

        protected abstract T ReadEntity(SqlDataReader reader);

        protected abstract void LoadBaseQueries();
    }
}
