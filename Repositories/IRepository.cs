using FileSharingService.Models;

namespace FileSharingService.Repository;

public interface IRepository
    {
        public Task SaveFileAsync(EntityFile entity);
        public Task<EntityFile> GetFileAsync(string fileName);
        public Task DeleteFileAsync(EntityFile entityFile);
        public Task DeleteFileTimeAsync(DateTime deleteTime);
}

