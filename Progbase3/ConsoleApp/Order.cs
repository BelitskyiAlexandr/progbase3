using System;

namespace ConsoleApp
{
    public class Order
    {
        public long id;
        public long userId;
        public DateTime createdAt;
        public string description;
        public double amount;

        public Order()
        {
            this.createdAt = DateTime.Now;
        }
    }
}