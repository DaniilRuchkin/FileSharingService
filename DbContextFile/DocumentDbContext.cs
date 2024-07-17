using Microsoft.EntityFrameworkCore;
using FileSharingService.Models;

namespace FileSharingService.DbContextFile;

public class DocumentDbContext(DbContextOptions<DocumentDbContext> options) : DbContext(options)
{
    public DbSet<Document> Files { get; set; }
}
