using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using ITSRunning.Models.Models;

namespace ITSRunning.DataAccess.Runners
{
    public class RunnerRepository : IRunnerRepository
    {
        private string _connectionString;
        public RunnerRepository(string cs)
        {
            _connectionString = cs;
        }
        public void Delete(int idRunner)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query =
                    @"DELETE FROM [dbo].[Runners]
                    WHERE Id = @id";

                connection.Query(query, new { type = idRunner });
            }
        }

        public IEnumerable<Runner> Get()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = @"SELECT [Id]
                                  ,[Username]
                                  ,[LastName]
                                  ,[FirstName]
                                  ,[BirthDate]
                                  ,[Gender]
                                  ,[PhotoUri]
                              FROM [dbo].[Runners]";

                return connection.Query<Runner>(query).ToList();
            }
        }

        public Runner Get(int idRunner)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = @"SELECT [Id]
                                  ,[Username]
                                  ,[LastName]
                                  ,[FirstName]
                                  ,[BirthDate]
                                  ,[Gender]
                                  ,[PhotoUri]
                              FROM [dbo].[Runners]
                              WHERE Id = @id";

                return connection.QueryFirstOrDefault<Runner>(query, new { id = idRunner });
            }
        }

        public Runner GetByUsername(string username)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = @"SELECT [Id]
                                  ,[Username]
                                  ,[LastName]
                                  ,[FirstName]
                                  ,[BirthDate]
                                  ,[Gender]
                                  ,[PhotoUri]
                            FROM [dbo].[Runners]
                            WHERE Username = @username";

                return connection.QueryFirstOrDefault<Runner>(query, new { username = username });
            }
        }

        public void Insert(Runner value)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query =
                    @"
                        INSERT INTO [dbo].[Runners]
                                ([Username]
                                ,[LastName]
                                ,[FirstName]
                                ,[BirthDate]
                                ,[Gender]
                                ,[PhotoUri])
                         VALUES
                               (@Username
                               ,@LastName
                               ,@FirstName
                               ,@BirthDate
                               ,@Gender
                               ,@PhotoUri)";

                connection.Query(query, value);
            }
        }

        public void Update(Runner value)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query =
                    @"UPDATE [dbo].[Runners]
                       SET [Username] = @Username
                          ,[LastName] = @LastName
                          ,[FirstName] = @FirstName
                          ,[BirthDate] = @BirthDate
                          ,[Gender] = @Gender
                          ,[PhotoUri] = @PhotoUri
                     WHERE Id = @Id";

                connection.Query(query, value);
            }
        }
    }
}
