using Domain.Publisher.Entities;

namespace Domain.Repositories;

public interface IExampleRepository
{
    bool Add(Example example);
    bool DeleteId(int id);
    List<Example> GetAll();
}
