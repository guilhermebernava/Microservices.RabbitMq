namespace Domain.Publisher.Entities;

public class Example
{
    public Example(int id, string name, string gender)
    {
        Id = id;
        Name = name;
        Gender = gender;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
}
