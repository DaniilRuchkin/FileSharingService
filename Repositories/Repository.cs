using FileSharingService.DbContextFile;
using FileSharingService.Models;
using Microsoft.EntityFrameworkCore;

namespace FileSharingService.Repository;

public class Repository(FileDbContext appDbContext) : IRepository
{
    public async Task<EntityFile> GetFileAsync(string fileName)
    {
        var getFile =  await appDbContext.Files.
            AsNoTracking().
            FirstOrDefaultAsync(name => name.UniqueName == fileName);

        return getFile!;
    }

    public async Task SaveFileAsync(EntityFile entityFile)
    {
        await appDbContext.Files.AddAsync(entityFile);

        await appDbContext.SaveChangesAsync();
    }

    public async Task DeleteFileAsync(EntityFile entityFile)
    {
        appDbContext.Files.Remove(entityFile);

        await appDbContext.SaveChangesAsync();
    }

    public async Task DeleteFileTimeAsync(DateTime deleteTime)
    {
        var filesToDelete = await appDbContext.Files
            .AsNoTracking()
            .Where(f => f.Time < deleteTime)
            .ToListAsync();

        foreach (var file in filesToDelete)
        {
            File.Delete(file.FilePath);

            appDbContext.Files.Remove(file);
        }

        await appDbContext.SaveChangesAsync();
    }
}

