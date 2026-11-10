using Entidades;

namespace Datos.IntegrationTests;

public class EfUsuarioRepositoryTests : MongoIntegrationTestBase
{
    [Fact]
    public void InsertarYObtenerPorNombre_PersisteElUsuario()
    {
        var usuario = new Usuario
        {
            NombreUsuario = $"usuario-{Guid.NewGuid():N}",
            PasswordHash = "hash-de-prueba"
        };

        using (UniversidadDbContext context = CrearContexto())
        {
            var repository = new EfUsuarioRepository(context);
            repository.Insertar(usuario);
        }

        using (UniversidadDbContext context = CrearContexto())
        {
            var repository = new EfUsuarioRepository(context);
            Usuario? guardado =
                repository.ObtenerPorNombre(usuario.NombreUsuario);

            Assert.NotNull(guardado);
            Assert.Equal(usuario.Id, guardado.Id);
            Assert.Equal("hash-de-prueba", guardado.PasswordHash);
            Assert.Equal(1, repository.Contar());
        }
    }
}
