using System.ComponentModel.DataAnnotations;

namespace PlatformService.HelpClasses;

public class EntityDto<T>
{
    public T Id { get; set; }
}