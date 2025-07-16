using System.Text.Json;
using Serilog;
namespace CoreProject.Services;

public class JsonService<T>{
    List<T> Items { get; }
    private static string fileName = $"{typeof(T).Name}.json";
    private string filePath;
    private readonly ILogger<JsonService<T>> _logger;

    public JsonService(IHostEnvironment env)
    {
        _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<JsonService<T>>();
        _logger.LogInformation($"start JsonService<{typeof(T).Name}> Constructor");
        filePath = Path.Combine(env.ContentRootPath, "Data", fileName);
        using (var jsonFile = File.OpenText(filePath))
        {
            Items = JsonSerializer.Deserialize<List<T>>(jsonFile.ReadToEnd(),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        _logger.LogInformation($"end JsonService<{typeof(T).Name}> Constructor");
    }

    public List<T> GetItems() => Items;

    public void saveToFile()
    {
        _logger.LogInformation($"start JsonService<{typeof(T).Name}> saveToFile");
        File.WriteAllText(filePath, JsonSerializer.Serialize(Items));
        _logger.LogInformation($"end JsonService<{typeof(T).Name}> saveToFile");
    }
}