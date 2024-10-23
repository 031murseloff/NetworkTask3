using Microsoft.EntityFrameworkCore;
using NetworkTask3.Models;

namespace NetworkTask3.Contexts;

public class UserDbContext:DbContext
{
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
=> optionsBuilder.UseSqlServer("Server=DESKTOP-T3OFGQN;Database=NetworkProgramingTask3;User Id=elsan;Password=admin1234;TrustServerCertificate=True;");


}
