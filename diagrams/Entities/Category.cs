namespace DefaultNamespace;

public class Category
{
    public Guid Id { get; set; }
    public string Title { get; set; }

    public List<DreamInCategory> Dreams { get; set; }
}