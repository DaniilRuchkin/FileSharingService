using FileSharingService.Models;

namespace FileSharingService.Repository;

public interface IFileRepository
{
    public Task SaveFileAsync(Document entity, CancellationToken cancellationToken);

    public Task<Document> GetFileAsync(string fileName, CancellationToken cancellationToken);

    public Task DeleteFileAsync(Document entityFile, CancellationToken cancellationToken);

    public Task<IEnumerable<Document>> GetFilesToDeleteAsync(DateTime deleteTime, CancellationToken cancellationToken);

    public Task SaveShangesAsync(CancellationToken cancellationToken);
}