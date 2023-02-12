using System.ComponentModel.DataAnnotations;

namespace PlatformService.HelpClasses;

public class Entity<T>
{   
    [Key]
    [Required]
    public T Id { get; set; }
}