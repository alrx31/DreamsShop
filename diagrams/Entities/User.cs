namespace Domain.Entities;

public class UserBase
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public Roles Role { get; set; }
}

public class ProduserUser
{
    public Produser Company { get; set; }
    public List<Dream> CreatedDreams { get; set; }
}

public class ConsumerUser 
{
    public List<Order> Orders { get; set; }
}