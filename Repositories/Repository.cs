using FileSharingService.DbContextFile;
using FileSharingService.Models;
using Microsoft.EntityFrameworkCore;

namespace FileSharingService.Repository;

public class Repository(DocumentDbContext appDbContext) : IRepository
{
    public async Task<Document> GetFileAsync(string fileName)
    {
        var getFile =  await appDbContext.Files.
            AsNoTracking().
            FirstOrDefaultAsync(name => name.UniqueName == fileName);

        return getFile!;
    }

    public async Task SaveFileAsync(Document entityFile)
    {
        await appDbContext.Files.AddAsync(entityFile);

        await appDbContext.SaveChangesAsync();
    }

    public async Task DeleteFileAsync(Document entityFile)
    {
        appDbContext.Files.Remove(entityFile);

        await appDbContext.SaveChangesAsync();
    }

    public async Task DeleteFileTimeAsync(DateTime deleteTime)
    {
        var filesToDelete = await appDbContext.Files
            .AsNoTracking()
            .Where(f => f.UploadFileTime < deleteTime)
            .ToListAsync();

        foreach (var file in filesToDelete)
        {
            System.IO.File.Delete(file.FilePath);

            appDbContext.Files.Remove(file);
        }

        await appDbContext.SaveChangesAsync();
    }
}

