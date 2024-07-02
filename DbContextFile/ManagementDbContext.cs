using Microsoft.EntityFrameworkCore;
using FileSharingService.Models;

namespace FileSharingService.DbContextFile;

public class ManagementDbContext(DbContextOptions<ManagementDbContext> options) : DbContext(options)
{
    public DbSet<EntityFile> Files { get; set; }
}
