using Domain.Publisher.Entities;

namespace Publisher.Services;

public interface IPublisherServices
{
   bool SendTo(ToDo data);
}
