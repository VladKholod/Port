using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;

using PortSimulator.Core.Entities;

using PortSimulator.DatabaseManager.Repositories.RepositoryAbstractions;

namespace PortSimulator.DatabaseManager.Repositories
{
    public sealed class CargoTypeRepository : BaseRepository<CargoType>, IRepository<CargoType>
    {
        public CargoTypeRepository()
        {
            LoadBaseQueries();
        }

        #region IRepository<T>
        public CargoType GetEntity(int id)
        {
            CargoType cargoType = null;

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
                        cargoType = ReadEntity(reader);
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid ID");
                }
            }

            return cargoType;
        }

        public async Task Save(CargoType entity)
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

        public List<CargoType> GetAll()
        {
            List<CargoType> cargoTypes = new List<CargoType>();

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
                        cargoTypes.Add(ReadEntity(reader));
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return cargoTypes;
        }
        #endregion

        #region BaseRepository
        protected override CargoType ReadEntity(SqlDataReader reader)
        {
            return new CargoType()
                        {
                            ID = int.Parse(reader[0].ToString().Trim()),
                            Name = reader[1].ToString().Trim()
                        };
        }

        protected override void LoadBaseQueries()
        {
            _queries.Add("Insert", "insert into CargoType (Name) values(@Name);");
            _queries.Add("Update", "update CargoType set Name = @Name where ID = @ID;");
            _queries.Add("Delete", "delete CargoType where ID = @ID;");
            _queries.Add("Select", "select * from CargoType where ID = @ID;");
            _queries.Add("Select All", "select * from CargoType;");
        }
        #endregion
    }
}
