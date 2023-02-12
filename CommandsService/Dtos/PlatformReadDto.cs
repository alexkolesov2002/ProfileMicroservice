using PlatformService.HelpClasses;

namespace CommandsService.Dtos;

public class PlatformReadDto :EntityDto<int>
{
    public string Name { get; set; }
}