using FileSharingService.DTO;
using FileSharingService.Models;

namespace FileSharingService.Services;

public interface IFileService
{
    public Task<FileDataDto> FileSaveAsync(CreateFileDto dtoFile);
    public Task<FileDataDto> DowloadFileAsync(string fileName);
    public Task<FileDataDto> DeleteFileAsync(FileDeleteDto fileDeleteDto);
}

