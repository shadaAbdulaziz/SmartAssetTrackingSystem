using Microsoft.EntityFrameworkCore;
using SmartAssetTrackingSystem.Models;

namespace SmartAssetTrackingSystem.Data;

public class AppDbContext : DbContext
{
    public DbSet<Office> Offices { get; set; }
    public DbSet<ComputerAsset> ComputerAssets { get; set; }
    public DbSet<MobileAsset> MobileAssets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\MSSQLLocalDB;Database=SmartAssetTrackingDb;Trusted_Connection=True;TrustServerCertificate=True;"
        );
    }

    
}