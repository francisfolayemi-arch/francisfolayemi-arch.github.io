using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Basic health-check endpoint — confirms the container itself is up.
app.MapGet("/", () => "DockerDemo is running inside a container.");

// Connects to the SQL Server container to prove the two containers can talk.
// Connection string comes from an environment variable set in docker-compose.yml,
// so nothing is hardcoded here.
app.MapGet("/db-check", async () =>
{
    var connectionString = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION")
        ?? "Server=sqlserver,1433;Database=master;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;";

    try
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command = new SqlCommand("SELECT @@VERSION", connection);
        var version = (string)(await command.ExecuteScalarAsync() ?? "unknown");
        return Results.Ok(new { status = "connected", version });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Could not connect to SQL Server: {ex.Message}");
    }
});

app.Run("http://0.0.0.0:8080");
