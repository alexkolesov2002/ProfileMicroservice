using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IMapper _mapper;

    public EventProcessor(IServiceScopeFactory serviceScopeFactory, IMapper mapper)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _mapper = mapper;
    }

    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);

        switch (eventType)
        {
            case EventType.PlatformPublished:
                AddPlatform(message);
                Console.WriteLine($"Add to database new platform from platform service: {message}");
                break;
            case EventType.Undetermined:
                Console.WriteLine("Could not determine the event type");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private EventType DetermineEvent(string notificationMessage)
    {
        Console.WriteLine("DetermineEvent");
        var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

        switch (eventType?.Event)
        {
            case "Platform_Published":
                Console.WriteLine("Platform Published event Detected");
                return EventType.PlatformPublished;
            default:
                Console.WriteLine("Could not determine the event type");
                return EventType.Undetermined;
        }
    }

    private void AddPlatform(string platformPublishedMessage)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<ICommandRepository>();

        var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

        try
        {
            var platform = _mapper.Map<Platform>(platformPublishedDto);
            platform.Id = 0;
            if (!repository.PlatformFromPlatformsServiceExits(platform.ExternalPlatformId))
            {
                repository.CreatePlatform(platform);
                repository.SaveChanges();
                Console.WriteLine("add new platform successful");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Not add new platform" + e);
        }
    }
}