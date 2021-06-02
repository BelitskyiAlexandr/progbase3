using System;

public class Order
{
    public long id;
    public long userId;
    public DateTime createdAt;
    public double amount;
    public Good[] goods;

    public Order()
    {
        this.createdAt = DateTime.Now;
        this.goods = new Good[0];
    }

    public Order(long userId)
    {
        this.userId = userId;
        this.createdAt = DateTime.Now;
        this.goods = new Good[0];
    }

    public Order(long userId, Good[] goods)
    {
        this.userId = userId;
        this.goods = goods;
        this.amount = SetAmount();
        this.createdAt = DateTime.Now;
    }

    public override string ToString()
    {
        return $"Order: [{id}] Created at:({createdAt.ToShortDateString()}) Price: {amount.ToString("####.00")}";
    }

    public string UserOrders()
    {
        return $"Order: [{id}] ({createdAt.ToString()})";
    }

    public void AddGoodToOrder(Good good)
    {
        Expand();
        this.goods[goods.Length - 1] = good;
    }

    private void Expand()
    {
        int oldCapacity = this.goods.Length;
        Good[] oldArray = this.goods;
        this.goods = new Good[oldCapacity + 1];
        System.Array.Copy(oldArray, this.goods, oldCapacity);
    }

    public double SetAmount()
    {
        double amount = 0;
        for (int i = 0; i < goods.Length; i++)
        {
            amount += goods[i].price;
        }
        this.amount = amount;
        return amount;
    }
}
