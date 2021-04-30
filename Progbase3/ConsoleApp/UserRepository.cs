using System;
using Microsoft.Data.Sqlite;

namespace ConsoleApp
{
    public class UserRepository
    {
        public SqliteConnection connection;
        public UserRepository(SqliteConnection connection)
        {
            this.connection = connection;
        }

        public bool UserExists(string username)
        {
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM users WHERE username = $username";
            command.Parameters.AddWithValue("$username", username);

            SqliteDataReader reader = command.ExecuteReader();

            bool result = reader.Read();

            connection.Close();
            return result;
        }

        public long Insert(User user)
        {
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
            @"
                INSERT INTO users (username, fullname, createdAt) 
                VALUES ($username, $fullname, $createdAt);
            
                SELECT last_insert_rowid();
            ";
            command.Parameters.AddWithValue("$username", user.username);
            command.Parameters.AddWithValue("$fullname", user.fullname);
            command.Parameters.AddWithValue("$createdAt", user.createdAt.ToString("o"));

            long newId = (long)command.ExecuteScalar();

            connection.Close();
            return newId;
        }

        public void AllRecords()                        //Невідомо чи буде використовуватись
        {
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM users";

            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                User user = new User();
                user.id = long.Parse(reader.GetString(0));
                user.username = reader.GetString(1);
                user.fullname = reader.GetString(2);
                user.createdAt = DateTime.Parse(reader.GetString(3));


                Console.WriteLine(user);
            }

            reader.Close();

            connection.Close();

        }

        public bool Delete(User user)
        {
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM users WHERE username = $username";
            command.Parameters.AddWithValue("$username", user.username);

            int nChanged = command.ExecuteNonQuery();

            connection.Close();
            return !(nChanged == 0);
        }

        private long GetCount()
        {
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT COUNT(*) FROM users";

            long count = (long)command.ExecuteScalar();
            return count;
        }

    }
}