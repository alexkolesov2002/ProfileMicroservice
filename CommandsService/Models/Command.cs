using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PlatformService.HelpClasses;

namespace CommandsService.Models;

public class Command : Entity<int>
{
    [Required]
    public string HowTo { get; set; }
    
    [Required]
    public string CommandLine { get; set; }
    
    [Required]
    public  int PlatformId { get; set; }
    
    [ForeignKey("PlatformId")]
    public Platform PlatformFk { get; set; }
}