namespace DAL.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    
    public Guid? ProducerId { get; set; }
    public Producer Producer { get; set; }
    
    public Roles Role { get; set; }
}