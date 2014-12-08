using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;

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
                        captain = ReadEntity(reader);
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid ID");
                }
            }
            
            return captain;
        }

        public async Task Save(Captain entity)
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

        public List<Captain> GetAll()
        {
            List<Captain> captains = new List<Captain>();

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
                            ID = int.Parse(reader[0].ToString().Trim()),
                            FirstName = reader[1].ToString().Trim(),
                            LastName = reader[2].ToString().Trim()
                        };
        }

        protected override void LoadBaseQueries() 
        {
            _queries.Add("Insert", "insert into Captain (FirstName, LastName) values(@FirstName, @LastName);");
            _queries.Add("Update", "update Captain set FirstName = @FirstName, LastName = @LastName where ID = @ID;");
            _queries.Add("Delete", "delete Captain where ID = @ID;");
            _queries.Add("Select", "select * from Captain where ID = @ID;");
            _queries.Add("Select All", "select * from Captain;");
        }
        #endregion
    }
}
