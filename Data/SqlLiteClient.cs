using iworfShop_backend_light.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace iworfShop_backend_light.Data;

public class SqlLiteClient : DbContext
{
    public DbSet<Config> Configs { get; set; }
    public DbSet<User> Users { get; set; }
}