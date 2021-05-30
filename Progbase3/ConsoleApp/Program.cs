using System;
using Microsoft.Data.Sqlite;

// insert, delete, get(all, one)



class Program
{
    static void Main(string[] args)
    {
        string databaseFileName = @"../../data/shop";
        SqliteConnection connection = new SqliteConnection($"Data Source={databaseFileName}");
        UserRepository userRepository = new UserRepository(connection);
        GoodRepository goodRepository = new GoodRepository(connection);
        OrderRepository orderRepository = new OrderRepository(connection);
        XmlProcess export = new XmlProcess();

        // export.XmlExport(goodRepository.GetExportGoods("a"), "./export.xml");
        // export.XmlImport("./export.xml", goodRepository);

        User user = new User("user user", "moderUser", "moderUser");
        Hashing.SignIn(user.username, user.password, userRepository);
    }
}

