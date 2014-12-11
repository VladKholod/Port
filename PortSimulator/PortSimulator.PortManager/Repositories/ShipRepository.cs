using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using PortSimulator.Core.Entities;
using PortSimulator.DatabaseManager.Repositories.RepositoryAbstractions;

namespace PortSimulator.DatabaseManager.Repositories
{
    public sealed class ShipRepository : BaseRepository<Ship>, IRepository<Ship>
    {
        public ShipRepository()
        {
            LoadBaseQueries();
        }

        #region IRepository<T>
        public Ship GetEntity(int id)
        {
            Ship ship = null;

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
                        ship = ReadEntity(reader);
                    }

                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid ID");
                }
            }

            return ship;
        }

        public async Task Save(Ship entity)
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
                    sqlCommand.Parameters.AddWithValue("@Capacity", entity.Capacity);
                    sqlCommand.Parameters.AddWithValue("@CreateDate", entity.CreateDate);
                    sqlCommand.Parameters.AddWithValue("@MaxDistance", entity.MaxDistance);
                    sqlCommand.Parameters.AddWithValue("@TeamCount", entity.TeamCount);
                    sqlCommand.Parameters.AddWithValue("@PortID", entity.PortId);

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

        public List<Ship> GetAll()
        {
            var ships = new List<Ship>();

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
                        ships.Add(ReadEntity(reader));
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return ships;
        }
        #endregion

        #region BaseRepository
        protected override Ship ReadEntity(SqlDataReader reader)
        {
            return new Ship()
                        {
                            Id = int.Parse(reader[0].ToString().Trim()),
                            Number = int.Parse(reader[1].ToString().Trim()),
                            Capacity = int.Parse(reader[2].ToString().Trim()),
                            CreateDate = DateTime.Parse(reader[3].ToString().Trim()),
                            MaxDistance = int.Parse(reader[4].ToString().Trim()),
                            TeamCount = int.Parse(reader[5].ToString().Trim()),
                            PortId = int.Parse(reader[6].ToString().Trim())
                        };
        }

        protected override void LoadBaseQueries()
        {
            Queries.Add("Insert", "insert into Ship " +
                "(Number, Capacity, CreateDate, MaxDistance, TeamCount, PortID) " +
                "values(@Number, @Capacity, @CreateDate, @MaxDistance, @TeamCount, @PortID);");

            Queries.Add("Update", "update Ship set " +
                "Number = @Number, Capacity = @Capacity, CreateDate = @CreateDate, " +
                "MaxDistance = @MaxDistance, TeamCount = @TeamCount, " +
                "PortID = @PortID where ID = @ID;");

            Queries.Add("Delete", "delete Ship where ID = @ID;");
            Queries.Add("Select", "select * from Ship where ID = @ID;");
            Queries.Add("Select All", "select * from Ship;");
        }
        #endregion
    }
}

