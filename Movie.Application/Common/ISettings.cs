namespace Movie.Application.Common;

public interface ISettings
{
    string ConnectionString { get; }
    string WebAppUrl { get; }
    string WebApiUrl { get; }
    string PokBaseUrl { get; }
}

public class Settings : ISettings
{
    public string ConnectionString { get; set; } =
        "Host=localhost;Database=movie_app;Username=postgres;password=root";
    
    public string WebAppUrl { get; set; } = "http://localhost:3000";
    public string WebApiUrl { get; set; } = "http://localhost:5031";

    public string PokBaseUrl { get; set; }

    public static Settings GetSettingsFromEnvironment()
    {
        var defaultSettings = new Settings();

        return new Settings
        {
            ConnectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") ??
                               defaultSettings.ConnectionString,
            WebAppUrl = Environment.GetEnvironmentVariable("WEB_APP_URL") ?? defaultSettings.WebAppUrl,
            WebApiUrl = Environment.GetEnvironmentVariable("WEB_API_URL") ?? defaultSettings.WebApiUrl,
            PokBaseUrl = Environment.GetEnvironmentVariable("POK_BASE_URL"),
        };
    }


    private static string GetEnvironmentValue(string name, string defaultValue) =>
        Environment.GetEnvironmentVariable(name) ?? defaultValue;
}