using Domain.Publisher.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Publisher.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ToDoController : ControllerBase
{
    [HttpPost]
    public IActionResult Add([FromBody] Example example) 
    { 
        return Ok();
    }

    [HttpDelete]
    public IActionResult Delete([FromBody] int id)
    {
        return Ok();
    }

    [HttpGet]
    public IActionResult GetAll([FromServices] IExampleRepository repository)
    {
        return Ok(repository.GetAll());
    }
}
