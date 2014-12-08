using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;

using PortSimulator.Core.Entities;

using PortSimulator.DatabaseManager.Repositories.RepositoryAbstractions;

namespace PortSimulator.DatabaseManager.Repositories
{
    public sealed class TripRepository : BaseRepository<Trip>, IRepository<Trip>
    {
        public TripRepository()
        {
            LoadBaseQueries();
        }

        #region IRepository<T>
        public Trip GetEntity(int id)
        {
            Trip trip = null;

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
                        trip = ReadEntity(reader);
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid ID");
                }
            }

            return trip;
        }

        public async Task Save(Trip entity)
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

                    sqlCommand.Parameters.AddWithValue("@StartDate", entity.StartDate);
                    sqlCommand.Parameters.AddWithValue("@EndDate", entity.EndDate);
                    sqlCommand.Parameters.AddWithValue("@ShipID", entity.ShipID);
                    sqlCommand.Parameters.AddWithValue("@CaptainID", entity.CaptainID);
                    sqlCommand.Parameters.AddWithValue("@PortFromID", entity.PortFromID);
                    sqlCommand.Parameters.AddWithValue("@PortToID", entity.PortToID);

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

        public List<Trip> GetAll()
        {
            List<Trip> trips = new List<Trip>();

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
                        trips.Add(ReadEntity(reader));
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return trips;
        }
        #endregion

        #region BaseRepository
        protected override Trip ReadEntity(SqlDataReader reader)
        {
            return new Trip()
                        {
                            ID = int.Parse(reader[0].ToString().Trim()),
                            StartDate = DateTime.Parse(reader[1].ToString().Trim()),
                            EndDate = DateTime.Parse(reader[2].ToString().Trim()),
                            ShipID = int.Parse(reader[3].ToString().Trim()),
                            CaptainID = int.Parse(reader[4].ToString().Trim()),
                            PortFromID = int.Parse(reader[5].ToString().Trim()),
                            PortToID = int.Parse(reader[6].ToString().Trim())
                        };
        }

        protected override void LoadBaseQueries()
        {
            _queries.Add("Insert", "insert into Trip " +
                "(StartDate, EndDate, ShipID, CaptainID, PortFromID, PortToID) " +
                "values(@StartDate, @EndDate, @ShipID, @CaptainID, @PortFromID, @PortToID);");

            _queries.Add("Update", "update Trip set " +
                "StartDate = @StartDate, EndDate = @EndDate, ShipID = @ShipID, " +
                "CaptainID = @CaptainID, PortFromID = @PortFromID, " +
                "PortToID = @PortToID where ID = @ID;");

            _queries.Add("Delete", "delete Trip where ID = @ID;");
            _queries.Add("Select", "select * from Trip where ID = @ID;");
            _queries.Add("Select All", "select * from Trip;");
        }
        #endregion
    }
}

