using FileSharingService.Models;

namespace FileSharingService.Repository;

public interface IRepository
{
    public Task SaveFileAsync(Document entity);
    public Task<Document> GetFileAsync(string fileName);
    public Task DeleteFileAsync(Document entityFile);
    public Task DeleteFileTimeAsync(DateTime deleteTime);
}