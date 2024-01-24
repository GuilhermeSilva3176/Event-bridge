using Event_bridge.Model;
using Microsoft.EntityFrameworkCore;

namespace Event_bridge.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


    public DbSet<UsuariosModel> Usuarios { get; set; }
}
