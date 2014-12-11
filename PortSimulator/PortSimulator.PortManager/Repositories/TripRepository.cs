using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
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
                        trip = ReadEntity(reader);
                    }

                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid ID");
                }
            }

            return trip;
        }

        public async Task Save(Trip entity)
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

                    sqlCommand.Parameters.AddWithValue("@StartDate", entity.StartDate);
                    sqlCommand.Parameters.AddWithValue("@EndDate", entity.EndDate);
                    sqlCommand.Parameters.AddWithValue("@ShipID", entity.ShipId);
                    sqlCommand.Parameters.AddWithValue("@CaptainID", entity.CaptainId);
                    sqlCommand.Parameters.AddWithValue("@PortFromID", entity.PortFromId);
                    sqlCommand.Parameters.AddWithValue("@PortToID", entity.PortToId);

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

        public List<Trip> GetAll()
        {
            var trips = new List<Trip>();

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
                            Id = int.Parse(reader[0].ToString().Trim()),
                            StartDate = DateTime.Parse(reader[1].ToString().Trim()),
                            EndDate = DateTime.Parse(reader[2].ToString().Trim()),
                            ShipId = int.Parse(reader[3].ToString().Trim()),
                            CaptainId = int.Parse(reader[4].ToString().Trim()),
                            PortFromId = int.Parse(reader[5].ToString().Trim()),
                            PortToId = int.Parse(reader[6].ToString().Trim())
                        };
        }

        protected override void LoadBaseQueries()
        {
            Queries.Add("Insert", "insert into Trip " +
                "(StartDate, EndDate, ShipID, CaptainID, PortFromID, PortToID) " +
                "values(@StartDate, @EndDate, @ShipID, @CaptainID, @PortFromID, @PortToID);");

            Queries.Add("Update", "update Trip set " +
                "StartDate = @StartDate, EndDate = @EndDate, ShipID = @ShipID, " +
                "CaptainID = @CaptainID, PortFromID = @PortFromID, " +
                "PortToID = @PortToID where ID = @ID;");

            Queries.Add("Delete", "delete Trip where ID = @ID;");
            Queries.Add("Select", "select * from Trip where ID = @ID;");
            Queries.Add("Select All", "select * from Trip;");
        }
        #endregion
    }
}

