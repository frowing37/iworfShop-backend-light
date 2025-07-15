using iworfShop_backend_light.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace iworfShop_backend_light.Data;

public class SqlLiteClient : DbContext
{
    public SqlLiteClient(DbContextOptions<SqlLiteClient> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
}