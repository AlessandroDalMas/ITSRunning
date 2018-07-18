using Dapper;
using ITSRunning.Models.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ITSRunning.DataAccess.Activities
{
    public class ActivityRepository : IActivityRepository
    {
        private string _connectionString;

        public ActivityRepository(string cs)
        {
            _connectionString = cs;
        }
        public void Delete(int idActivity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query =
                    @"DELETE FROM [dbo].[Activities]
                    WHERE Id = @id";
                connection.Query(query, new { id = idActivity });
            }
        }

        public IEnumerable<Activity> Get()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = @"SELECT [Id]
                                  ,[Name]
                                  ,[IdRunner]
                                  ,[CreationDate]
                                  ,[Location]
                                  ,[Type]
                                  ,[UriMatch]
                                  ,[State]
                              FROM [dbo].[Activities]";

                return connection.Query<Activity>(query).ToList();
            }
        }

        public Activity Get(int idActivity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = @"SELECT [Id]
                                  ,[Name]
                                  ,[IdRunner]
                                  ,[CreationDate]
                                  ,[Location]
                                  ,[Type]
                                  ,[UriMatch]
                                  ,[State]
                              FROM [dbo].[Activities]
                              WHERE Id = @id";


                return connection.QueryFirstOrDefault<Activity>(query, new { id = idActivity });
            }
        }

        public IEnumerable<Activity> GetByTypeAndUsername(int type, string username)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //TODO check
                var query = @"SELECT a.[Id]
                                  ,a.[Name]
                                  ,a.[IdRunner]
                                  ,a.[CreationDate]
                                  ,a.[Location]
                                  ,a.[Type]
                                  ,a.[UriMatch]
                                  ,a.[State]
                              FROM [dbo].[Activities] a JOIN [dbo].[Runners] r ON a.IdRunner = r.Id
                              WHERE a.Type = @type AND r.Username = @username";
                
                return connection.Query<Activity>(query, new { type = type, username = username }).ToList();
            }
        }

        public void Insert(Activity value)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query =
                    @"
                        INSERT INTO [dbo].[Activities]
                                ([Name]
                                ,[IdRunner]
                                ,[CreationDate]
                                ,[Location]
                                ,[Type]
                                ,[UriMatch]
                                ,[State])
                         VALUES
                               (@Name
                               ,@IdRunner
                               ,@CreationDate
                               ,@Location
                               ,@Type
                               ,@UriMatch
                               ,@State)";
                connection.Query(query, value);
            }
        }

        public void Update(Activity value)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query =
                    @"UPDATE [dbo].[Activities]
                       SET [Name] = @Name
                          ,[IdRunner] = @IdRunner
                          ,[CreationDate] = @CreationDate
                          ,[Location] = @Location
                          ,[Type] = @Type
                          ,[UriMatch] = @UriMatch
                          ,[State] = @State
                     WHERE Id = @Id";

                connection.Query(query, value);
            }
        }
    }
}
