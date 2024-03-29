using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

    public class UserRepository
    {
        public SqliteConnection connection;
        public UserRepository(SqliteConnection connection)
        {
            this.connection = connection;
        }

        public bool UserExistsByUsername(string username)
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

        public User GetUserByUsername(string username)
        {
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM users WHERE username = $username";
            command.Parameters.AddWithValue("$username", username);

            SqliteDataReader reader = command.ExecuteReader();
            
            User user = new User();
            if(reader.Read())
            {
                user.id = long.Parse(reader.GetString(0));
                user.username = reader.GetString(1);
                user.password = reader.GetString(2);
                user.fullname = reader.GetString(3);
                user.createdAt = DateTime.Parse(reader.GetString(4));
                user.role = reader.GetString(5);
            }

            connection.Close();
            return user;
        }

        public long Insert(User user)
        {
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
            @"
                INSERT INTO users (username, password, fullname, createdAt, role) 
                VALUES ($username, $password, $fullname, $createdAt, $role);
            
                SELECT last_insert_rowid();
            ";
            command.Parameters.AddWithValue("$username", user.username);
            command.Parameters.AddWithValue("$password", user.password);
            command.Parameters.AddWithValue("$fullname", user.fullname);
            command.Parameters.AddWithValue("$createdAt", user.createdAt.ToString("o"));
            command.Parameters.AddWithValue("$role", user.role);

            long newId = (long)command.ExecuteScalar();

            connection.Close();
            return newId;
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

        public List<User> AllRecords()                       
        {
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM users";

            SqliteDataReader reader = command.ExecuteReader();
            List<User> users = new List<User>();
            while (reader.Read())
            {
                User user = new User();
                user.id = long.Parse(reader.GetString(0));
                user.username = reader.GetString(1);
                user.fullname = reader.GetString(3);
                user.createdAt = DateTime.Parse(reader.GetString(4));

                users.Add(user);
            }

            reader.Close();

            connection.Close();
            return users;
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
