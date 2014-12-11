using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
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

            var query = Queries["Select"];

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var sqlCommand = connection.CreateCommand();

                try
                {
                    sqlCommand.CommandText = query;

                    sqlCommand.Parameters.AddWithValue("@ID", id);

                    var reader = sqlCommand.ExecuteReader();
                    if (reader.Read())
                    {
                        port = ReadEntity(reader);
                    }

                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid ID");
                }
            }

            return port;
        }

        public async Task Save(Port entity)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();

                var sqlCommand = connection.CreateCommand();
                sqlCommand.Transaction = transaction;

                try
                {
                    string query;
                    if (!entity.IsNew())
                    {
                        query = Queries["Update"];
                        sqlCommand.CommandText = query;
                        sqlCommand.Parameters.AddWithValue("@ID", entity.Id);
                    }
                    else
                    {
                        query = Queries["Insert"];
                        sqlCommand.CommandText = query;
                    }

                    sqlCommand.Parameters.AddWithValue("@Name", entity.Name);
                    sqlCommand.Parameters.AddWithValue("@CityID", entity.CityId);

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
            var query = Queries["Delete"];

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();

                var sqlCommand = connection.CreateCommand();
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
            var ports = new List<Port>();

            var query = Queries["Select All"];

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var sqlCommand = connection.CreateCommand();

                try
                {
                    sqlCommand.CommandText = query;

                    var reader = sqlCommand.ExecuteReader();

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
                            Id = int.Parse(reader[0].ToString().Trim()),
                            Name = reader[1].ToString().Trim(),
                            CityId = int.Parse(reader[2].ToString().Trim())
                        };
        }

        protected override void LoadBaseQueries()
        {
            Queries.Add("Insert", "insert into Port " +
                "(Name, CityID) values(@Name, @CityID);");

            Queries.Add("Update", "update Port set " +
                "Name = @Name, CityID = @CityID where ID = @ID;");

            Queries.Add("Delete", "delete Port where ID = @ID;");
            Queries.Add("Select", "select * from Port where ID = @ID;");
            Queries.Add("Select All", "select * from Port;");
        }
        #endregion
    }
}
