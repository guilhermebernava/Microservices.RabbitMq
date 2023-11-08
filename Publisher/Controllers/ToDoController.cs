using Domain.Publisher.Entities;
using Domain.Publisher.Enums;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Publisher.Services;

namespace Publisher.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ToDoController : ControllerBase
{
    [HttpPost]
    public IActionResult Add([FromBody] Example example, [FromServices] IPublisherServices services) 
    { 
        return services.SendTo(new ToDo(EType.Add, example, DateTime.Now)) ? Ok() : BadRequest();
    }

    [HttpDelete]
    public IActionResult Delete([FromBody] int id, [FromServices] IPublisherServices services)
    {
        return services.SendTo(id,EType.Delete) ? Ok() : BadRequest();
    }

    [HttpGet]
    public IActionResult GetAll([FromServices] IExampleRepository repository)
    {
        return Ok(repository.GetAll());
    }
}
