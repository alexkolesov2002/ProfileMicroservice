using System.ComponentModel.DataAnnotations;
using PlatformService.HelpClasses;

namespace PlatformService.Models;

public class Platform : Entity<int>
{
   [Required]
   public string Name { get; set; }

   [Required]
   public string Publisher { get; set; }

   [Required]
   public string Cost { get; set; }
} 