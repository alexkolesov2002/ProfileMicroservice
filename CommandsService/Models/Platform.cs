using System.ComponentModel.DataAnnotations;
using PlatformService.HelpClasses;

namespace CommandsService.Models;

public class Platform: Entity<int>
{
    [Required]
    public  int ExternalPlatformId { get; set; }
     
    [Required] 
    public string Name { get; set; }
    
    public ICollection<Command> Commands { get; set; } = new List<Command>();
}