using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using ITSRunning.Models.Models;
using ITSRunning.Models.Telemetries;

namespace ITSRunning.DataAccess.Telemetries
{
    public class TelemetryRepository : ITelemetryRepository
    {
        private string _connectionString;
        public TelemetryRepository(string cs)
        {
            _connectionString = cs;
        }
        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query =
                    @"DELETE FROM [dbo].[Telemetries]
                    WHERE Id = @id";

                connection.Query(query, new { id = id });
            }
        }

        public void DeleteAll(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query =
                    @"DELETE FROM [dbo].[Telemetries]
                    WHERE IdActivity = @id";

                connection.Query(query, new { id = id });
            }
        }

        public IEnumerable<Telemetry> Get()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = @"SELECT [Id]
                                  ,[Latitude]
                                  ,[Longitude]
                                  ,[Instant]
                                  ,[IdRunner]
                                  ,[IdActivity]
                                  ,[UriSelfie]
                              FROM [dbo].[Telemetries]";

                return connection.Query<Telemetry>(query).ToList();
            }
        }

        public Telemetry Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TelemetryDetails> GetAll(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = @"SELECT Latitude
                                  ,Longitude
                                  ,Instant
                                  ,UriSelfie
                              FROM Telemetries
                              WHERE IdActivity = @id
                              ORDER BY Instant";

                return connection.Query<TelemetryDetails>(query, new { id = id }).ToList();
            }
        }

        public void Insert(Telemetry telemetry)
        {
            throw new NotImplementedException();
        }

        public void Insert(Telemetry telemetry, string username)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query =
                    @"INSERT INTO Telemetries (Latitude, Longitude, Instant, IdRunner, IdActivity)
                        SELECT @Latitude, @Longitude, @Instant, r.Id, @IdActivity
                        FROM Runners r 
                        WHERE r.Username = @Username";

                connection.Query(query, new {
                    telemetry.Latitude,
                    telemetry.Longitude,
                    telemetry.Instant,
                    telemetry.IdActivity,
                    Username = username});
            }
        }

        public void Update(Telemetry value)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query =
                    @"UPDATE Telemetries
                       SET Id = @Id
                          ,Latitude = @Latitude
                          ,Longitude = @Longitude
                          ,Instant = @Instant
                          ,IdRunner = @IdRunner
                          ,IdActivity = @IdActivity
                          ,UriSelfie = @UriSelfie
                     WHERE Id = @Id";

                connection.Query(query, value);
            }
        }

        public void Update(string uriPic, int idActivity, DateTime instant)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query =
                    @"declare @id as int;
                    SELECT TOP 1 @id = Id
		                    FROM Telemetries
		                    WHERE IdActivity = @IdActivity AND Instant < @Instant
		                    ORDER BY Instant DESC
                    UPDATE Telemetries
	                    SET UriSelfie = @UriSelfie
	                    WHERE Id = @id";

                connection.Query(query, new { UriSelfie = uriPic, IdActivity = idActivity, Instant = instant });
            }
        }
    }
}
