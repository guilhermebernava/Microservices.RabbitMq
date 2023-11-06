using Domain.Publisher.Entities;
using Domain.Repositories;

namespace Infra.Repositories;

public class ExampleRepository : IExampleRepository
{
    public ExampleRepository()
    {
        _examples = new List<Example>();
    }
    private IList<Example> _examples { get; set; }

    public bool Add(Example example)
    {
        _examples.Add(example);
        return true;
    }

    public bool DeleteId(int id)
    {
        var exist = _examples.FirstOrDefault(x => x.Id == id);

        if (exist == null)
            return false;
        return _examples.Remove(exist);
    }

    public List<Example> GetAll()
    {
        return _examples.ToList();
    }
}
