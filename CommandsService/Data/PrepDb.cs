using CommandsService.SyncDataServices.Grpc;

namespace CommandsService.Data;

public static class PrepDb
{
    public static void PrepPopulation(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();

        SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>()!,
            serviceScope.ServiceProvider.GetService<IPlatformDataClient>()!);
    }

    private static  void SeedData(AppDbContext context, IPlatformDataClient grpcDataClient)
    {
        var platforms =  grpcDataClient.ReturnAllPlatforms().GetAwaiter().GetResult();
        foreach (var platform in platforms)
        {
            if (!context.Platforms.Any(x => x.ExternalPlatformId == platform.ExternalPlatformId))
            {
                context.Platforms.Add(platform);
                context.SaveChanges();
            }
        }
    }
}