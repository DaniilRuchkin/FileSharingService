using FileSharingService.DbContextFile;
using FileSharingService.Models;
using Microsoft.EntityFrameworkCore;

namespace FileSharingService.Repository;

public class FileRepository(DocumentDbContext appDbContext) : IFileRepository
{
    public async Task<Document> GetFileAsync(string fileName, CancellationToken cancellationToken)
    {
        var getFile =  await appDbContext.Files.
            AsNoTracking().
            FirstOrDefaultAsync(name => name.UniqueName == fileName, cancellationToken);

        return getFile!;
    }

    public async Task SaveFileAsync(Document entityFile, CancellationToken cancellationToken)
    {
        await appDbContext.Files.AddAsync(entityFile, cancellationToken);

        await appDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteFileAsync(Document entityFile, CancellationToken cancellationToken)
    {
        appDbContext.Files.Remove(entityFile);

        await appDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Document>> GetFilesToDeleteAsync(DateTime deleteTime, int pageIndex, int pageSize, CancellationToken cancellationToken)
    {
        var filesToDelete = await appDbContext.Files
            .AsNoTracking()
            .Where(f => f.UploadFileTime < deleteTime)
            .Skip(pageIndex * pageIndex)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return filesToDelete;
    }
}