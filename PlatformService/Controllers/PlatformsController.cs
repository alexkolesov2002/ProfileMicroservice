using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepository _platformRepository;
    private readonly IMapper _mapper;

    public PlatformsController(IPlatformRepository platformRepository, IMapper mapper)
    {
        _platformRepository = platformRepository;
        _mapper = mapper;
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
    public ActionResult<PlatformReadDto> CreatePlatform(PlatformCreateDto input)
    {
        Console.WriteLine("Пошло получение платформы");
        var platform = _mapper.Map<Platform>(input);
        _platformRepository.CreatePlatform(platform);
        _platformRepository.SaveChanges();

        var platformRead = _mapper.Map<PlatformReadDto>(platform);
        return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformRead.Id }, platformRead); 
    }
}