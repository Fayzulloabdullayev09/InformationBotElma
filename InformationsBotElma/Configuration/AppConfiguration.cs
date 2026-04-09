namespace InformationsBotElma.Configuration;

public static class AppConfiguration
{
    public static BotOptions Bot { get; } = new();
    public static DatabaseOptions Database { get; } = new();
    public static SeedOptions Seed { get; } = new();
}

public sealed class BotOptions
{
    public string Token { get; init; } = "8710077154:AAHajhVoAbTJ-q5Vq3s0wdPah7mSec1VM3o";
}

public sealed class DatabaseOptions
{
   public string ConnectionString { get; init; } =
        "Host=localhost;Port=5432;Database=informationsbotelma;Username=postgres;Password=1234abD.";
}

public sealed class SeedOptions
{
    public IReadOnlyCollection<string> AdminUserNames { get; init; } =
        new[] { "@fayzullo_a_a","@vohid_mehmonov","@wrong_nodir" };

    // Oddiy user usernamelar shu ro'yxatga yoziladi.
    public IReadOnlyCollection<string> RegularUserNames { get; init; } =
        new[] { "Task Operator", "Guest User" };
}
