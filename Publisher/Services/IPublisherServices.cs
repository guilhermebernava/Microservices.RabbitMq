using Domain.Publisher.Entities;
using Domain.Publisher.Enums;

namespace Publisher.Services;

public interface IPublisherServices
{
   bool SendTo(ToDo data);
    bool SendTo(int data,EType type);
}
