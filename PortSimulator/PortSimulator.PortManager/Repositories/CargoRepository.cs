using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;

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
                        cargo = ReadEntity(reader);
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid ID");
                }
            }

            return cargo;
        }

        public async Task Save(Cargo entity)
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

                    sqlCommand.Parameters.AddWithValue("@Number", entity.Number);
                    sqlCommand.Parameters.AddWithValue("@Weight", entity.Weight);
                    sqlCommand.Parameters.AddWithValue("@Price", entity.Price);
                    sqlCommand.Parameters.AddWithValue("@InsurancePrice", entity.InsurancePrice);
                    sqlCommand.Parameters.AddWithValue("@CargoTypeID", entity.CargoTypeID);
                    sqlCommand.Parameters.AddWithValue("@TripID", entity.TripID);

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

        public List<Cargo> GetAll()
        {
            List<Cargo> cargos = new List<Cargo>();

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
                            ID = int.Parse(reader[0].ToString().Trim()),
                            Number = int.Parse(reader[1].ToString().Trim()),
                            Weight = int.Parse(reader[2].ToString().Trim()),
                            Price = int.Parse(reader[3].ToString().Trim()),
                            InsurancePrice = int.Parse(reader[4].ToString().Trim()),
                            CargoTypeID = int.Parse(reader[5].ToString().Trim()),
                            TripID = int.Parse(reader[6].ToString().Trim())
                        };
        }

        protected override void LoadBaseQueries()
        {
            _queries.Add("Insert", "insert into Cargo " +
                "(Number, Weight, Price, InsurancePrice, CargoTypeID, TripID) " +
                "values(@Number, @Weight, @Price, @InsurancePrice, @CargoTypeID, @TripID);");

            _queries.Add("Update", "update Cargo set " + 
                "Number = @Number, Weight = @Weight, Price = @Price, " +
                "InsurancePrice = @InsurancePrice, CargoTypeID = @CargoTypeID, " +
                "TripID = @TripID where ID = @ID;");

            _queries.Add("Delete", "delete Cargo where ID = @ID;");
            _queries.Add("Select", "select * from Cargo where ID = @ID;");
            _queries.Add("Select All", "select * from Cargo;");
        }
        #endregion
    }
}
