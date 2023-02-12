using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data;

//TODO ИСПОЛЬЗОВАЛ РАСШИРЕНИЯ ПО СОВЕТУ АНТОНА
public static class PrepDb
{
    public static void PrepPopulation(this IApplicationBuilder app, bool isProd)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>()!, isProd);
    }

    private static void SeedData(AppDbContext context, bool isProd)
    {
        try
        {
            if (isProd)
            {
                Console.WriteLine("Происходит попытка автоматического применения миграций");
                context.Database.Migrate();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }


        if (!context.Platforms.Any())
        {
            Console.WriteLine("Данных нет");
            context.Platforms.AddRange(
                new Platform() { Name = ".Net", Publisher = "Microsoft", Cost = "Free" },
                new Platform() { Name = "ZERO", Publisher = "ЦП", Cost = "300" },
                new Platform() { Name = "Docker", Publisher = "docker", Cost = "250" });
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("Данные уже есть");
        }
    }
}