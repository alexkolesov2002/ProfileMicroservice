using System.ComponentModel.DataAnnotations;
using PlatformService.HelpClasses;

namespace CommandsService.Dtos;

public class CommandCreateDto
{
    [Required]
    public string HowTo { get; set; }
   
    [Required]
    public string CommandLine { get; set; }
}