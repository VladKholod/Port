using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;

using PortSimulator.Core.Entities;

using PortSimulator.DatabaseManager.Repositories.RepositoryAbstractions;

namespace PortSimulator.DatabaseManager.Repositories
{
    public sealed class CityRepository : BaseRepository<City>, IRepository<City>
    {
        public CityRepository()
        {
            LoadBaseQueries();
        }

        #region IRepository<T>
        public City GetEntity(int id)
        {
            City city = null;

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
                        city = ReadEntity(reader);
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid ID");
                }
            }

            return city;
        }

        public async Task Save(City entity)
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

        public List<City> GetAll()
        {
            List<City> cities = new List<City>();

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
                        cities.Add(ReadEntity(reader));
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return cities;
        }
        #endregion

        #region BaseRepository
        protected override City ReadEntity(SqlDataReader reader)
        {
            return new City()
                        {
                            ID = int.Parse(reader[0].ToString().Trim()),
                            Name = reader[1].ToString().Trim()
                        };
        }

        protected override void LoadBaseQueries()
        {
            _queries.Add("Insert", "insert into City (Name) values(@Name);");
            _queries.Add("Update", "update City set Name = @Name where ID = @ID;");
            _queries.Add("Delete", "delete City where ID = @ID;");
            _queries.Add("Select", "select * from City where ID = @ID;");
            _queries.Add("Select All", "select * from City;");
        }
        #endregion
    }
}
