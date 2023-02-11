using System.Text;
using System.Text.Json;
using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http;

public class HttpCommandDataClient : ICommandDataClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task SendPlatformToCommand(PlatformReadDto input)
    {
        var httpContent = new StringContent(
            JsonSerializer.Serialize(input),
            Encoding.UTF8,
            "application/json"
        );
        var response = await _httpClient.PostAsync($"{_configuration["CommandService"]}", httpContent);
        
        Console.WriteLine(response.IsSuccessStatusCode
            ? "Коннект со службой команд установлен"
            : "Чего то при коннекте со службой команд пошло не так");
    }
}