using FileSharingService.Models;

namespace FileSharingService.Repository;

public interface IRepository
{
    public Task SaveFileAsync(Models.File entity);
    public Task<Models.File> GetFileAsync(string fileName);
    public Task DeleteFileAsync(Models.File entityFile);
    public Task DeleteFileTimeAsync(DateTime deleteTime);
}