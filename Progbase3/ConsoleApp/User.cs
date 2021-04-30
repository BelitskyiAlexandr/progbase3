using System;

namespace ConsoleApp
{
    public class User
    {
        public long id;
        public string username;
        public string fullname;
        public DateTime createdAt;

        public User()
        {
            this.createdAt = DateTime.Now;
        }
    }
}