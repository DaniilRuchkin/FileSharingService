using Microsoft.EntityFrameworkCore;
using FileSharingService.Models;

namespace FileSharingService.DbContextFile;

public class FileDbContext(DbContextOptions<FileDbContext> options) : DbContext(options)
{
    public DbSet<Models.File> Files { get; set; }
}
