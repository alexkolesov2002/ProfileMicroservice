using PlatformService.HelpClasses;

namespace PlatformService.Dtos;

public class PlatformPublishedDto : EntityDto<int>
{
    public string Name { get; set; }

    public string Event { get; set; }
}