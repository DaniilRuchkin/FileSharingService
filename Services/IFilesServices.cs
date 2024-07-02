using FileSharingService.DTO;
using FileSharingService.Models;

namespace FileSharingService.Services;

public interface IFilesServices
{
    public Task<string> FileSaveAsync(CreateFileDto dtoFile);
    public Task<EntityFile> DowloadFileAsync(string fileName);
    public Task<bool> DeleteFileAsync(FileDeleteDto fileDeleteDto);
}

