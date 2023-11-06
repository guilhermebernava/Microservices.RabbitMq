using Domain.Publisher.Enums;

namespace Domain.Publisher.Entities;

public class ToDo
{
    public ToDo(EType type, Example example, DateTime date)
    {
        Type = type;
        Example = example;
        Date = date;
    }

    public EType Type { get; set; }
    public Example Example { get; set; }
    public DateTime Date { get; set; }
}
