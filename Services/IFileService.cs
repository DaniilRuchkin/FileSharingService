using FileSharingService.DTO;
using FileSharingService.Models;

namespace FileSharingService.Services;

public interface IFileService
{
    public Task<string> FileSaveAsync(CreateFileDto dtoFile);
    public Task<Document> DowloadFileAsync(string fileName);
    public Task<bool> DeleteFileAsync(FileDeleteDto fileDeleteDto);
}

