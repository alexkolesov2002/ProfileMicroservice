using CommandsService.Models;

namespace CommandsService.Data;

public class CommandRepository : ICommandRepository
{
    private readonly AppDbContext _appDbContext;

    public CommandRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public bool SaveChanges()
    {
        return _appDbContext.SaveChanges() >= 0;
    }

    public IEnumerable<Platform> GetAllPlatforms()
    {
        return _appDbContext.Platforms.ToList();
    }

    public void CreatePlatform(Platform platform)
    {
        if (platform is  null)
        {
            throw new ArgumentNullException(nameof(platform));
        }

        _appDbContext.Platforms.Add(platform);
    }

    public bool PlatformExits(int platformId)
    {
        return _appDbContext.Platforms.Any(x => x.Id.Equals(platformId));
    }

    public bool PlatformFromPlatformsServiceExits(int externalPlatformId)
    {
        return _appDbContext.Platforms.Any(x => x.ExternalPlatformId.Equals(externalPlatformId));
    }

    public IEnumerable<Command> GetCommandsForPlatform(int platformId)
    {
        return _appDbContext.Commands.Where(x => x.PlatformId.Equals(platformId))
            .OrderBy(x=>x.PlatformFk.Name);
    }
    
    public Command GetCommand(int platformId ,int commandId)
    {
        return _appDbContext.Commands.FirstOrDefault(x => x.Id == commandId 
                                                          && x.PlatformId == platformId)!;
    }

    public void CreateCommand(int platformId, Command command)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        command.PlatformId = platformId;
        _appDbContext.Add(command);
    }
}