using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

    public PlatformsController(IPlatformRepository platformRepository, IMapper mapper,
        ICommandDataClient commandDataClient)
    {
        _platformRepository = platformRepository;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
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
        Console.WriteLine("Пошло получение платформы");
        var platform = _mapper.Map<Platform>(input);
        _platformRepository.CreatePlatform(platform);
        _platformRepository.SaveChanges();

        var platformRead = _mapper.Map<PlatformReadDto>(platform);
        try
        {
            await _commandDataClient.SendPlatformToCommand(platformRead);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformRead.Id }, platformRead);
    }
}