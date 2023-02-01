using PlatformService.Models;

namespace PlatformService.Data;

public class PlatformRepository: IPlatformRepository
{
    private readonly AppDbContext _context;
    
    public PlatformRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public bool SaveChanges()
    {
        return (_context.SaveChanges() >= 0);
    }

    public IEnumerable<Platform> GetAllPlatforms()
    {
        return _context.Platforms.ToList();
    }

    public Platform? GetPlatformById(int id)
    {
        return _context.Platforms.FirstOrDefault(x => x.Id.Equals(id));
    }

    public void CreatePlatform(Platform platform)
    {
        if (platform is not null)
        {
            _context.Platforms.Add(platform);
        }
        else
        {
            throw new ArgumentNullException();
        }
        
    }
}