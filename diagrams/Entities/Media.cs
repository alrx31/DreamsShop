namespace Domain.Entities;

public class Media
{
    public int Id { get; set; }
    public string File_Name { get; set; }
    public string File_Extension { get; set; }
    public string File_Size { get; set; }
    public string File_Path { get; set; }
    
    public byte[] File { get; set; }
}