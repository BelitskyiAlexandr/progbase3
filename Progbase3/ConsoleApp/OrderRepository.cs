using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Data.Sqlite;

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
    public void AddGoodsOrders(Order order)
    {
        for (int i = 0; i < order.goods.Length; i++)
        {
            connection.Open();
            
            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
            @"
                INSERT INTO goods_orders (goodId, orderId) 
                VALUES ($goodId, $orderId);
            ";
            command.Parameters.AddWithValue("$goodId", order.goods[i].id);
            command.Parameters.AddWithValue("$orderId", order.id);

            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public long Insert(Order order)
    {
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText =
        @"
                INSERT INTO orders (userId, createdAt, amount) 
                VALUES ($userId, $createdAt, $amount);
            
                SELECT last_insert_rowid();
            ";
        command.Parameters.AddWithValue("$userId", order.userId);
        command.Parameters.AddWithValue("$createdAt", order.createdAt);
        command.Parameters.AddWithValue("$amount", order.amount);

        long newId = (long)command.ExecuteScalar();

        connection.Close();
        return newId;
    }

    public bool Delete(Order order)
    {
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"DELETE FROM orders WHERE id = $id";
        command.Parameters.AddWithValue("$id", order.id);

        int nChanged = command.ExecuteNonQuery();

        connection.Close();
        return !(nChanged == 0);
    }

    public Good[] DeleteGoodFromOrder(Order order, Good good)
    {
        List<Good> goods = new List<Good>(order.goods);
        for(int i = 0; i < order.goods.Length; i++)
        {
            if(goods[i] == good)
            {
                goods.RemoveAt(i);
            }
        }
        order.goods = goods.ToArray();
        return order.goods;
    }

    public Order[] GetAllUserOrdersById(long userId)
    {
        List<Order> ordersList = new List<Order>();
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM orders WHERE userId = $userId";
        command.Parameters.AddWithValue("$userId", userId);

        SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Order order = new Order();
            order.id = int.Parse(reader.GetString(0));
            order.userId = long.Parse(reader.GetString(1));
            order.createdAt = DateTime.Parse(reader.GetString(2));
            order.amount = double.Parse(reader.GetString(3), CultureInfo.InvariantCulture); ;
            ordersList.Add(order);
        }
        Order[] orders = ordersList.ToArray();

        connection.Close();
        return orders;
    }

    public Order[] GetOrdersByGoodId(long goodId)
    {
        List<long> ordersIdList = new List<long>();
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM goods_orders WHERE goodId = $goodId";
        command.Parameters.AddWithValue("$goodId", goodId);

        SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            long orderId = long.Parse(reader.GetString(1));
            ordersIdList.Add(orderId);
        }
        connection.Close();

        List<Order> ordersList = new List<Order>();

        foreach (var orderId in ordersIdList)
        {
            Order order = GetById(orderId);
            ordersList.Add(order);
        }

        Order[] orders = ordersList.ToArray();

        return orders;
    }

    public Order GetById(long id)
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();

        command.CommandText = @"SELECT * FROM orders WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);

        SqliteDataReader reader = command.ExecuteReader();
        Order order = new Order();
        if (reader.Read())
        {
            order.id = long.Parse(reader.GetString(0));
            order.userId = long.Parse(reader.GetString(1));
            order.createdAt = DateTime.Parse(reader.GetString(2));
            order.amount = double.Parse(reader.GetString(3), CultureInfo.InvariantCulture);
        }
        connection.Close();

        return order;
    }
}
