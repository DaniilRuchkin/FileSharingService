using FileSharingService.DbContextFile;
using FileSharingService.Models;
using Microsoft.EntityFrameworkCore;

namespace FileSharingService.Repository;

public class FileRepository(DocumentDbContext appDbContext) : IFileRepository
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

    public async Task<IEnumerable<Document>> GetFilesToDeleteAsync(DateTime deleteTime)
    {
        var filesToDelete = await appDbContext.Files
            .AsNoTracking()
            .Where(f => f.UploadFileTime < deleteTime)
            .ToListAsync();

        return filesToDelete;
    }

    public async Task SaveShangesAsync()
    {
        await appDbContext.SaveChangesAsync();
    }
}