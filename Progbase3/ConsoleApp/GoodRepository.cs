using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Data.Sqlite;


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
                INSERT INTO goods (name, description, inStock, price) 
                VALUES ($name, $description, $inStock, price);
            
                SELECT last_insert_rowid();
            ";
        command.Parameters.AddWithValue("$name", good.name);
        command.Parameters.AddWithValue("$description", good.description);
        command.Parameters.AddWithValue("$inStock", good.inStock);
        command.Parameters.AddWithValue("$price", good.price);


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

    public Good[] GetGoodsByOrderId(long orderId)
    {
        List<long> goodIdList = new List<long>();
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM goods_orders WHERE orderId = $orderId";
        command.Parameters.AddWithValue("$orderId", orderId);

        SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            long goodId = long.Parse(reader.GetString(0));
            goodIdList.Add(goodId);
        }
        connection.Close();

        List<Good> goodsList = new List<Good>();

        foreach (var goodId in goodIdList)
        {
            Good good = GetById(goodId);
            goodsList.Add(good);
        }

        Good[] goods = goodsList.ToArray();

        return goods;
    }

    public Good GetById(long id)
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM goods WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);
        SqliteDataReader reader = command.ExecuteReader();
        Good good = new Good();
        if (reader.Read())
        {
            good.id = long.Parse(reader.GetString(0));
            good.name = reader.GetString(1);
            good.description = reader.GetString(2);
            good.inStock = bool.Parse(reader.GetString(3));
            good.price = double.Parse(reader.GetString(4), CultureInfo.InvariantCulture);
        }
        connection.Close();

        return good;
    }

    public Good[] GetExportGoods(string substring)
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM goods WHERE name LIKE '%' || $value || '%' ";  
        command.Parameters.AddWithValue("$value", substring);

        SqliteDataReader reader = command.ExecuteReader();

        List<Good> goodsList = new List<Good>();
        while (reader.Read())
        {
            Good good = new Good();
            good.id = long.Parse(reader.GetString(0));
            good.name = reader.GetString(1);
            good.description = reader.GetString(2);
            good.inStock = bool.Parse(reader.GetString(3));
            good.price = double.Parse(reader.GetString(4), CultureInfo.InvariantCulture);
            goodsList.Add(good);
        }
        reader.Close();
        connection.Close();

        Good[] goods = goodsList.ToArray();

        return goods;
    }

    public int GetTotalPages()
    {
        const int pageSize = 5;
        return (int)Math.Ceiling(this.GetCount() / (double)pageSize);
    }
    private long GetCount() //for GetTotalPages
    {
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT COUNT(*) FROM goods";

        long count = (long)command.ExecuteScalar();
        return count;
    }

    public List<Good> GetPage(int pageNumber)
    {
        if( pageNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber));
        }
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM goods LIMIT 5 OFFSET 5*($pageNumber-1)";
        command.Parameters.AddWithValue("$pageNumber", pageNumber);

        SqliteDataReader reader = command.ExecuteReader();
        List<Good> goods = new List<Good>();

        while (reader.Read())
        {
            Good good = new Good();
            good.id = int.Parse(reader.GetString(0));
            good.name = reader.GetString(1);
            good.description = reader.GetString(2);
            good.inStock = bool.Parse(reader.GetString(3));
            good.price = double.Parse(reader.GetString(4), CultureInfo.InvariantCulture);
            goods.Add(good);
        }

        reader.Close();
        connection.Close();

        return goods;
    }

    public bool Update(long id, Good good)
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = $"UPDATE goods SET name = $name, description = $description, inStock = $inStock, price = $price WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);
        command.Parameters.AddWithValue("$name", good.name);
        command.Parameters.AddWithValue("$description", good.description);
        command.Parameters.AddWithValue("$inStock", good.inStock.ToString());
        command.Parameters.AddWithValue("$price", good.price);
        int rowChange = command.ExecuteNonQuery();

        connection.Close();
        if (rowChange == 0)
        {
            return false;
        }

        return true;
    }

    public List<Good> GetAll()
    {
        connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM goods";

            SqliteDataReader reader = command.ExecuteReader();
            List<Good> goods = new List<Good>();
            while (reader.Read())
            {
                Good good = new Good();
                good.id = long.Parse(reader.GetString(0));
                good.name = reader.GetString(1);
                good.description = reader.GetString(2);
                good.inStock = bool.Parse(reader.GetString(3));
                good.price = double.Parse(reader.GetString(4), CultureInfo.InvariantCulture);

                goods.Add(good);
            }

            reader.Close();

            connection.Close();
            return goods;
    }
}
