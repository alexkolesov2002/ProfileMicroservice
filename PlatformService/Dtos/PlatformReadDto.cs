using System.ComponentModel.DataAnnotations;
using PlatformService.HelpClasses;

namespace PlatformService.Dtos;

public class PlatformReadDto : EntityDto<int>
{
    public string Name { get; set; }

    public string Publisher { get; set; }
    
    public string Cost { get; set; }
}