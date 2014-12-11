using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
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
                        cargoType = ReadEntity(reader);
                    }

                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid ID");
                }
            }

            return cargoType;
        }

        public async Task Save(CargoType entity)
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

        public List<CargoType> GetAll()
        {
            var cargoTypes = new List<CargoType>();

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
                            Id = int.Parse(reader[0].ToString().Trim()),
                            Name = reader[1].ToString().Trim()
                        };
        }

        protected override void LoadBaseQueries()
        {
            Queries.Add("Insert", "insert into CargoType (Name) values(@Name);");
            Queries.Add("Update", "update CargoType set Name = @Name where ID = @ID;");
            Queries.Add("Delete", "delete CargoType where ID = @ID;");
            Queries.Add("Select", "select * from CargoType where ID = @ID;");
            Queries.Add("Select All", "select * from CargoType;");
        }
        #endregion
    }
}
