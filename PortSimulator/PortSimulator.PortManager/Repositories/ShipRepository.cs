using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;

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
                        ship = ReadEntity(reader);
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid ID");
                }
            }

            return ship;
        }

        public async Task Save(Ship entity)
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
                    sqlCommand.Parameters.AddWithValue("@Capacity", entity.Capacity);
                    sqlCommand.Parameters.AddWithValue("@CreateDate", entity.CreateDate);
                    sqlCommand.Parameters.AddWithValue("@MaxDistance", entity.MaxDistance);
                    sqlCommand.Parameters.AddWithValue("@TeamCount", entity.TeamCount);
                    sqlCommand.Parameters.AddWithValue("@PortID", entity.PortID);

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

        public List<Ship> GetAll()
        {
            List<Ship> ships = new List<Ship>();

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
                            ID = int.Parse(reader[0].ToString().Trim()),
                            Number = int.Parse(reader[1].ToString().Trim()),
                            Capacity = int.Parse(reader[2].ToString().Trim()),
                            CreateDate = DateTime.Parse(reader[3].ToString().Trim()),
                            MaxDistance = int.Parse(reader[4].ToString().Trim()),
                            TeamCount = int.Parse(reader[5].ToString().Trim()),
                            PortID = int.Parse(reader[6].ToString().Trim())
                        };
        }

        protected override void LoadBaseQueries()
        {
            _queries.Add("Insert", "insert into Ship " +
                "(Number, Capacity, CreateDate, MaxDistance, TeamCount, PortID) " +
                "values(@Number, @Capacity, @CreateDate, @MaxDistance, @TeamCount, @PortID);");

            _queries.Add("Update", "update Ship set " +
                "Number = @Number, Capacity = @Capacity, CreateDate = @CreateDate, " +
                "MaxDistance = @MaxDistance, TeamCount = @TeamCount, " +
                "PortID = @PortID where ID = @ID;");

            _queries.Add("Delete", "delete Ship where ID = @ID;");
            _queries.Add("Select", "select * from Ship where ID = @ID;");
            _queries.Add("Select All", "select * from Ship;");
        }
        #endregion
    }
}

