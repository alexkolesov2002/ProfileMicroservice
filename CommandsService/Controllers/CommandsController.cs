using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[Route("api/c/platforms/{platformId:int}/[controller]")]
[ApiController]
public class CommandsController : ControllerBase
{
    private readonly ICommandRepository _commandRepository;
    private readonly IMapper _mapper;

    public CommandsController(ICommandRepository commandRepository, IMapper mapper)
    {
        _commandRepository = commandRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
    {
        Console.WriteLine($" Пришедший из URL Id {platformId}");
        if (_commandRepository.PlatformExits(platformId))
        {
            return Ok(_mapper.Map<List<CommandReadDto>>(_commandRepository.GetCommandsForPlatform(platformId)));
        }

        return NotFound();
    }
    
    [HttpGet("{commandId:int}", Name = "GetCommandForPlatform")]
    public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
    {
        Console.WriteLine($" Пришедший из URL platformId {platformId}, Пришедший из URL commandId {commandId}");
        if (!_commandRepository.PlatformExits(platformId))
        {
            return NotFound();
        }

        var command = _commandRepository.GetCommand(platformId, commandId);

        if (command is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<CommandReadDto>(command));
    }
    
    [HttpPost]
    public ActionResult<CommandReadDto> CreatePlatform(int platformId, CommandCreateDto input)
    {
        Console.WriteLine($" Пришедший из URL platformId {platformId}");
        if (!_commandRepository.PlatformExits(platformId))
        {
            return NotFound();
        }

        var command = _mapper.Map<Command>(input);
        
        _commandRepository.CreateCommand(platformId, command);
        _commandRepository.SaveChanges();

        var commandReadDto = _mapper.Map<CommandReadDto>(command);

        return CreatedAtRoute(nameof(GetCommandForPlatform), new { platformId = platformId, commandId = command.Id }, commandReadDto);
    }
}