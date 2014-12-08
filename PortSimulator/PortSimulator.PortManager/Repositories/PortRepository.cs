using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;

using PortSimulator.Core.Entities;

using PortSimulator.DatabaseManager.Repositories.RepositoryAbstractions;

namespace PortSimulator.DatabaseManager.Repositories
{
    public sealed class PortRepository : BaseRepository<Port>, IRepository<Port>
    {
        public PortRepository()
        {
            LoadBaseQueries();
        }

        #region IRepository<T>
        public Port GetEntity(int id)
        {
            Port port = null;

            string query = _queries["Select"];

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand sqlCommand = connection.CreateCommand();

                try
                {
                    sqlCommand.CommandText = query;

                    sqlCommand.Parameters.AddWithValue("@ID", id);

                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    if (reader.Read())
                    {
                        port = ReadEntity(reader);
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid ID");
                }
            }

            return port;
        }

        public async Task Save(Port entity)
        {
            string query = string.Empty;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.Transaction = transaction;

                try
                {
                    if (!entity.IsNew())
                    {
                        query = _queries["Update"];
                        sqlCommand.CommandText = query;
                        sqlCommand.Parameters.AddWithValue("@ID", entity.ID);
                    }
                    else
                    {
                        query = _queries["Insert"];
                        sqlCommand.CommandText = query;
                    }

                    sqlCommand.Parameters.AddWithValue("@Name", entity.Name);
                    sqlCommand.Parameters.AddWithValue("@CityID", entity.CityID);

                    await sqlCommand.ExecuteNonQueryAsync();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    transaction.Rollback();
                }
            }
        }

        public async Task Delete(int id)
        {
            string query = _queries["Delete"];

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.Transaction = transaction;

                try
                {
                    sqlCommand.CommandText = query;

                    sqlCommand.Parameters.AddWithValue("@ID", id);

                    await sqlCommand.ExecuteNonQueryAsync();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    transaction.Rollback();
                }
            }
        }

        public List<Port> GetAll()
        {
            List<Port> ports = new List<Port>();

            string query = _queries["Select All"];

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand sqlCommand = connection.CreateCommand();

                try
                {
                    sqlCommand.CommandText = query;

                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        ports.Add(ReadEntity(reader));
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return ports;
        }
        #endregion

        #region BaseRepository
        protected override Port ReadEntity(SqlDataReader reader)
        {
            return new Port()
                        {
                            ID = int.Parse(reader[0].ToString().Trim()),
                            Name = reader[1].ToString().Trim(),
                            CityID = int.Parse(reader[2].ToString().Trim())
                        };
        }

        protected override void LoadBaseQueries()
        {
            _queries.Add("Insert", "insert into Port " +
                "(Name, CityID) values(@Name, @CityID);");

            _queries.Add("Update", "update Port set " +
                "Name = @Name, CityID = @CityID where ID = @ID;");

            _queries.Add("Delete", "delete Port where ID = @ID;");
            _queries.Add("Select", "select * from Port where ID = @ID;");
            _queries.Add("Select All", "select * from Port;");
        }
        #endregion
    }
}
