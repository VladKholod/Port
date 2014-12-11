using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using PortSimulator.Core.Entities;
using PortSimulator.DatabaseManager.Repositories.RepositoryAbstractions;

namespace PortSimulator.DatabaseManager.Repositories
{
    public sealed class CargoRepository : BaseRepository<Cargo>, IRepository<Cargo>
    {
        public CargoRepository()
        {
            LoadBaseQueries();
        }

        #region IRepository<T>
        public Cargo GetEntity(int id)
        {
            Cargo cargo = null;

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
                        cargo = ReadEntity(reader);
                    }

                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid ID");
                }
            }

            return cargo;
        }

        public async Task Save(Cargo entity)
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

                    sqlCommand.Parameters.AddWithValue("@Number", entity.Number);
                    sqlCommand.Parameters.AddWithValue("@Weight", entity.Weight);
                    sqlCommand.Parameters.AddWithValue("@Price", entity.Price);
                    sqlCommand.Parameters.AddWithValue("@InsurancePrice", entity.InsurancePrice);
                    sqlCommand.Parameters.AddWithValue("@CargoTypeID", entity.CargoTypeId);
                    sqlCommand.Parameters.AddWithValue("@TripID", entity.TripId);

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

        public List<Cargo> GetAll()
        {
            var cargos = new List<Cargo>();

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
                        cargos.Add(ReadEntity(reader));
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return cargos;
        }
        #endregion

        #region BaseRepository
        protected override Cargo ReadEntity(SqlDataReader reader)
        {
            return new Cargo()
                        {
                            Id = int.Parse(reader[0].ToString().Trim()),
                            Number = int.Parse(reader[1].ToString().Trim()),
                            Weight = int.Parse(reader[2].ToString().Trim()),
                            Price = int.Parse(reader[3].ToString().Trim()),
                            InsurancePrice = int.Parse(reader[4].ToString().Trim()),
                            CargoTypeId = int.Parse(reader[5].ToString().Trim()),
                            TripId = int.Parse(reader[6].ToString().Trim())
                        };
        }

        protected override void LoadBaseQueries()
        {
            Queries.Add("Insert", "insert into Cargo " +
                "(Number, Weight, Price, InsurancePrice, CargoTypeID, TripID) " +
                "values(@Number, @Weight, @Price, @InsurancePrice, @CargoTypeID, @TripID);");

            Queries.Add("Update", "update Cargo set " + 
                "Number = @Number, Weight = @Weight, Price = @Price, " +
                "InsurancePrice = @InsurancePrice, CargoTypeID = @CargoTypeID, " +
                "TripID = @TripID where ID = @ID;");

            Queries.Add("Delete", "delete Cargo where ID = @ID;");
            Queries.Add("Select", "select * from Cargo where ID = @ID;");
            Queries.Add("Select All", "select * from Cargo;");
        }
        #endregion
    }
}
