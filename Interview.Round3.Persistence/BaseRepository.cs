using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Interview.Round3.Persistence
{
    public abstract class BaseRepository
    {
        #region Properties
        protected string ConnectionString { get; }
        #endregion

        #region Constructor

        protected BaseRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }
        #endregion

        #region Read A Single Item
        // This method can be used by derived classes to read anything that is a class.
        // The main reason for having these methods is to keep the code in the repositories 
        // clean and easy to read.  It's also annoying to type it over and over.
        protected TResult ReadSingle<TResult>(string sql, Action<SqlParameterCollection> addParameters, Func<SqlDataReader, TResult> read)
            where TResult : class
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = sql;

                        // Using the null conditional operator and Invoke makes it so
                        // that I don't have to write: if(addparameters != null)
                        addParameters?.Invoke(command.Parameters);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            return reader.Read()
                                ? read(reader)
                                : (TResult)null;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // This is pretty terrible exception handling, but error handling is not the focus here.
                // If you know of a better way to handle this and you have time...
                Console.WriteLine(e);
                throw;
            }
        }

        protected async Task<TResult> ReadSingleAsync<TResult>(string sql, Action<SqlParameterCollection> addParameters, Func<SqlDataReader, TResult> read)
            where TResult : class
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = sql;

                        addParameters?.Invoke(command.Parameters);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            return await reader.ReadAsync()
                                ? read(reader)
                                : (TResult)null;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region Read Many Items
        protected List<TResult> ReadMany<TResult>(string sql, Action<SqlParameterCollection> addParameters, Func<SqlDataReader, TResult> read)
            where TResult : class
        {
            try
            {
                List<TResult> result = new List<TResult>();

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = sql;

                        addParameters?.Invoke(command.Parameters);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.Add(read?.Invoke(reader));
                            }
                        }
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        protected async Task<List<TResult>> ReadManyAsync<TResult>(string sql, Action<SqlParameterCollection> addParameters, Func<SqlDataReader, TResult> read)
            where TResult : class
        {
            try
            {
                List<TResult> result = new List<TResult>();

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = sql;

                        addParameters?.Invoke(command.Parameters);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                result.Add(read?.Invoke(reader));
                            }
                        }
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion
    }
}
