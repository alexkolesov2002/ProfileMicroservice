 using AutoMapper;
 using CommandsService.Data;
 using CommandsService.Dtos;
 using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[Route("api/c/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly ICommandRepository _commandRepository;
    private readonly IMapper _mapper;

    public PlatformsController(ICommandRepository commandRepository, IMapper mapper)
    {
        _commandRepository = commandRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        Console.WriteLine("Get Platforms from CommandService");
        
        var platforms = _commandRepository.GetAllPlatforms();
        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
    }

    [HttpPost]
    public ActionResult TestInputConnection()
    {
        Console.WriteLine("Я принял запроос на себя из сервиса платформ");
        
        return Ok("Я принял запроос на себя из сервиса платформ");
    }
}