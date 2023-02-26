using AutoMapper;
using CommandsService.Models;
using Grpc.Net.Client;
using PlatformService;

namespace CommandsService.SyncDataServices.Grpc;

public class PlatformDataClient : GrpcPlatform.GrpcPlatformClient, IPlatformDataClient
{

    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public PlatformDataClient(IConfiguration configuration, IMapper mapper)
    {
        _configuration = configuration;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Platform>> ReturnAllPlatforms()
    {
        var platformServiceAddress = _configuration["GrpcPlatform"];
        Console.WriteLine($"Grpc connection to {platformServiceAddress}");

        var channel = GrpcChannel.ForAddress(platformServiceAddress ?? string.Empty);
        var client = new GrpcPlatform.GrpcPlatformClient(channel);
        var request = new GetAllRequest();

        try
        {
            var reply = await client.GetAllPlatformsAsync(request);
            return _mapper.Map<IEnumerable<Platform>>(reply.Platform);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
        
    }
}