using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[Route("api/c/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    [HttpPost]
    public ActionResult TestInputConnection()
    {
        Console.WriteLine("Я принял запроос на себя из сервиса платформ");
        
        return Ok("Я принял запроос на себя из сервиса платформ");
    }
}