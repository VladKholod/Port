using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
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
                        city = ReadEntity(reader);
                    }

                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid ID");
                }
            }

            return city;
        }

        public async Task Save(City entity)
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

        public List<City> GetAll()
        {
            var cities = new List<City>();

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
                            Id = int.Parse(reader[0].ToString().Trim()),
                            Name = reader[1].ToString().Trim()
                        };
        }

        protected override void LoadBaseQueries()
        {
            Queries.Add("Insert", "insert into City (Name) values(@Name);");
            Queries.Add("Update", "update City set Name = @Name where ID = @ID;");
            Queries.Add("Delete", "delete City where ID = @ID;");
            Queries.Add("Select", "select * from City where ID = @ID;");
            Queries.Add("Select All", "select * from City;");
        }
        #endregion
    }
}
