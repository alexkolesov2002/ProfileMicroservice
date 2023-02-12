using PlatformService.HelpClasses;

namespace CommandsService.Dtos;

public class CommandReadDto : EntityDto<int>
{
    public string HowTo { get; set; }
    
    public string CommandLine { get; set; }
    
    public  int PlatformId { get; set; }
}