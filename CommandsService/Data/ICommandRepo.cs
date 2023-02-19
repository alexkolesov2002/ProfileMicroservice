using CommandsService.Models;

namespace CommandsService.Data;

public interface ICommandRepository
{
    bool SaveChanges();

    IEnumerable<Platform> GetAllPlatforms();

    void CreatePlatform(Platform platform);

    bool PlatformExits(int platformId);

    bool PlatformFromPlatformsServiceExits(int externalPlatformId);
    
    //Commands
    IEnumerable<Command> GetCommandsForPlatform(int platformId);

    Command GetCommand(int platformId, int commandId);

    void CreateCommand(int platformId, Command command);



}