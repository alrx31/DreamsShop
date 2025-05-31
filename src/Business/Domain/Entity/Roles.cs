namespace Domain.Entity;

[Flags]
public enum Roles
{
    Consumer = 0b001,
    Provider = 0b010,
    Admin = 0b100,
}