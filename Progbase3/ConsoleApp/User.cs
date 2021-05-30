using System;

public class User
{
    public long id;
    public string username;
    public string password;
    public string fullname;
    public DateTime createdAt;
    public string ordersList;   //
    public string role;
    public Order[] orders;

    public User(string fullname, string username, string password)
    {
        this.fullname = fullname;
        this.username = username;
        this.createdAt = DateTime.Now;
        this.role = "user";

        this.password = password;
    }
    public User()
    {
        this.id = 0;
        this.username = "";
        this.fullname = "";
        this.role = "";
        this.password = "";
    }

    public void GetRole(string role)
    {
        this.role = role;
    }

    public override string ToString()
    {
        return $"User: [{id}] username: {username} fullname: {fullname}\r\n({createdAt.ToString()})";
    }
}
