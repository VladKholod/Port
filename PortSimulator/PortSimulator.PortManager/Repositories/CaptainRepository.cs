using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using PortSimulator.Core.Entities;
using PortSimulator.DatabaseManager.Repositories.RepositoryAbstractions;

namespace PortSimulator.DatabaseManager.Repositories
{
    public sealed class CaptainRepository : BaseRepository<Captain>, IRepository<Captain>
    {
        public CaptainRepository()
        {
            LoadBaseQueries();
        }

        #region IRepository<T>
        public Captain GetEntity(int id)
        {
            Captain captain = null;

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
                        captain = ReadEntity(reader);
                    }

                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid ID");
                }
            }
            
            return captain;
        }

        public async Task Save(Captain entity)
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

                    sqlCommand.Parameters.AddWithValue("@FirstName", entity.FirstName);
                    sqlCommand.Parameters.AddWithValue("@LastName", entity.LastName);

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

        public List<Captain> GetAll()
        {
            var captains = new List<Captain>();

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
                        captains.Add(ReadEntity(reader));
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return captains;
        }
        #endregion

        #region BaseRepository
        protected override Captain ReadEntity(SqlDataReader reader)
        {
            return new Captain()
                        {
                            Id = int.Parse(reader[0].ToString().Trim()),
                            FirstName = reader[1].ToString().Trim(),
                            LastName = reader[2].ToString().Trim()
                        };
        }

        protected override void LoadBaseQueries() 
        {
            Queries.Add("Insert", "insert into Captain (FirstName, LastName) values(@FirstName, @LastName);");
            Queries.Add("Update", "update Captain set FirstName = @FirstName, LastName = @LastName where ID = @ID;");
            Queries.Add("Delete", "delete Captain where ID = @ID;");
            Queries.Add("Select", "select * from Captain where ID = @ID;");
            Queries.Add("Select All", "select * from Captain;");
        }
        #endregion
    }
}
