using InformationsBotElma.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace InformationsBotElma.Data;

public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
{
    public MyDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();

        // Migration ishlatishda connection string shu config'dan olinadi.
        optionsBuilder.UseNpgsql(AppConfiguration.Database.ConnectionString);

        return new MyDbContext(optionsBuilder.Options);
    }
}
