using System;

public class Order
{
    public long id;
    public long userId;
    public DateTime createdAt;
    public string description;
    public double amount;
    public Good[] goods;

    public Order()
    {
        this.createdAt = DateTime.Now;
    }

    public Order(long userId, Good[] goods)
    {
        this.userId = userId;
        this.goods = goods;
        double amount = 0;
        string description = null;
        for (int i = 0; i < goods.Length; i++)
        {
            amount += goods[i].price;
            description += "\r\n\t" + goods[i].ToString();
        }
        this.amount = amount;
        this.description = description;
        this.createdAt = DateTime.Now;
    }

    public override string ToString()
    {
        return $"Order: [{id}] User: {userId}  ({createdAt.ToString()})\r\n\tPrice: {amount} \r\n\tdescription: {description}";
    }

    public string UserOrders()
    {
        return $"Order: [{id}] ({createdAt.ToString()})";
    }
}
