using Datos;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Datos.IntegrationTests;

public abstract class MongoIntegrationTestBase : IDisposable
{
    private readonly IMongoClient _mongoClient;
    private readonly string _databaseName;

    protected MongoIntegrationTestBase()
    {
        string connectionString =
            Environment.GetEnvironmentVariable("MONGODB_TEST_CONNECTION")
            ?? "mongodb://localhost:27017";

        _mongoClient = new MongoClient(connectionString);
        _databaseName = $"UniversidadTests_{Guid.NewGuid():N}";
    }

    protected UniversidadDbContext CrearContexto()
    {
        DbContextOptions<UniversidadDbContext> options =
            new DbContextOptionsBuilder<UniversidadDbContext>()
                .UseMongoDB(_mongoClient, _databaseName)
                .Options;

        return new UniversidadDbContext(options);
    }

    public void Dispose()
    {
        _mongoClient.DropDatabase(_databaseName);
        GC.SuppressFinalize(this);
    }
}
