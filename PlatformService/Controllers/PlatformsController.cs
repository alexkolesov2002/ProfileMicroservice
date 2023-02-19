using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepository _platformRepository;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;
    private readonly IMessageBusClient _messageBusClient;

    public PlatformsController(IPlatformRepository platformRepository, IMapper mapper,
        ICommandDataClient commandDataClient, IMessageBusClient messageBusClient)
    {
        _platformRepository = platformRepository;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
        _messageBusClient = messageBusClient;
    }

    [HttpGet()]
    public ActionResult<IEnumerable<PlatformReadDto>> GetAllPlatforms()
    {
        Console.WriteLine("Пошло получение платформ");
        var platforms = _mapper.Map<IEnumerable<PlatformReadDto>>(_platformRepository.GetAllPlatforms());
        return Ok(platforms);
    }

    [HttpGet("{id}", Name = "GetPlatformById")]
    public ActionResult<PlatformReadDto> GetPlatformById(int id)
    {
        Console.WriteLine("Пошло получение платформы");
        var platform = _mapper.Map<PlatformReadDto>(_platformRepository.GetPlatformById(id));
        if (platform is not null)
            return Ok(platform);

        return NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto input)
    {
        Console.WriteLine("Пошло создание платформы");
        var platform = _mapper.Map<Platform>(input);
        _platformRepository.CreatePlatform(platform);
        _platformRepository.SaveChanges();

        var platformRead = _mapper.Map<PlatformReadDto>(platform);
        try
        {
            //await _commandDataClient.SendPlatformToCommand(platformRead);

            var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platform);
            platformPublishedDto.Event = "Platform_Published";
            _messageBusClient.PublishNewPlatform(platformPublishedDto);
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        try
        {
            var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platform);
            platformPublishedDto.Event = "Platform_Published";
            _messageBusClient.PublishNewPlatform(platformPublishedDto);
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }


        return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformRead.Id }, platformRead);
    }
}