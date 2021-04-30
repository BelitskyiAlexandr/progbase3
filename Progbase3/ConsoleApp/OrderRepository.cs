using Microsoft.Data.Sqlite;

namespace ConsoleApp
{
    public class OrderRepository
    {
         public SqliteConnection connection;
        public OrderRepository(SqliteConnection connection)
        {
            this.connection = connection;
        }

        public bool OrderExists(long id)
        {
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM orders WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);

            SqliteDataReader reader = command.ExecuteReader();

            bool result = reader.Read();

            connection.Close();
            return result;
        }

        public long Insert(Order order)
        {
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
            @"
                INSERT INTO orders (description, amount) 
                VALUES ($description, $amount);
            
                SELECT last_insert_rowid();
            ";
            command.Parameters.AddWithValue("$amount", order.amount);
            command.Parameters.AddWithValue("$description", order.description);

            long newId = (long)command.ExecuteScalar();

            connection.Close();
            return newId;
        }

        public bool Delete(Order order)
        {
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM orders WHERE id = $id";
            command.Parameters.AddWithValue("$ide", order.id);

            int nChanged = command.ExecuteNonQuery();

            connection.Close();
            return !(nChanged == 0);
        }
    }
}