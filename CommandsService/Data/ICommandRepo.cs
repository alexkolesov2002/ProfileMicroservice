using CommandsService.Models;

namespace CommandsService.Data;

public interface ICommandRepository
{
    bool SaveChanges();

    IEnumerable<Platform> GetAllPlatforms();

    void CreatePlatform(Platform platform);

    bool PlatformExits(int platformId);
    
    //Commands
    IEnumerable<Command> GetCommandsForPlatform(int platformId);

    Command GetCommand(int platformId);

    void CreateCommand(int platformId, Command command);



}