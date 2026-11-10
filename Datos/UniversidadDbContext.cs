using Entidades;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Datos;

public class UniversidadDbContext : DbContext
{
    public UniversidadDbContext(
        DbContextOptions<UniversidadDbContext> options)
        : base(options)
    {
    }

    public DbSet<Alumno> Alumnos => Set<Alumno>();
    public DbSet<Curso> Cursos => Set<Curso>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Alumno>().ToCollection("Alumnos");
        modelBuilder.Entity<Curso>().ToCollection("Cursos");
        modelBuilder.Entity<Usuario>().ToCollection("Usuarios");
    }
}