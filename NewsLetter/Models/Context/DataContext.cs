using Microsoft.EntityFrameworkCore;
namespace NewsLetter.Models.Context;

public class DataContext : DbContext
{
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options) :
        base(options)
    {
    }

    public DbSet<News> News { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Topic> Topic { get; set; }
    public DbSet<User> Users { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("dataConnection");
            optionsBuilder.UseNpgsql(connectionString);

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
    }
}