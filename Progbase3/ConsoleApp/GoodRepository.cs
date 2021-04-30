using System;
using Microsoft.Data.Sqlite;

namespace ConsoleApp
{
    public class GoodRepository
    {
        public SqliteConnection connection;
        public GoodRepository(SqliteConnection connection)
        {
            this.connection = connection;
        }

        public bool GoodExists(string name)
        {
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM goods WHERE name = $name";
            command.Parameters.AddWithValue("$name", name);

            SqliteDataReader reader = command.ExecuteReader();

            bool result = reader.Read();

            connection.Close();
            return result;
        }

        public long Insert(Good good)
        {
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
            @"
                INSERT INTO goods (name, description) 
                VALUES ($name, $description);
            
                SELECT last_insert_rowid();
            ";
            command.Parameters.AddWithValue("$name", good.name);
            command.Parameters.AddWithValue("$description", good.description);

            long newId = (long)command.ExecuteScalar();

            connection.Close();
            return newId;
        }

        public bool Delete(Good good)
        {
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM goods WHERE name = $name";
            command.Parameters.AddWithValue("$name", good.name);

            int nChanged = command.ExecuteNonQuery();

            connection.Close();
            return !(nChanged == 0);
        }
    }
}