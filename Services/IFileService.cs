using FileSharingService.DTO;

namespace FileSharingService.Services;

public interface IFileService
{
    public Task<FileDataDto> FileSaveAsync(CreateFileDto dtoFile, CancellationToken cancellationToken);

    public Task<FileDataDto> DowloadFileAsync(string fileName, CancellationToken cancellationToken);

    public Task<FileDataDto> DeleteFileAsync(FileDeleteDto fileDeleteDto, CancellationToken cancellationToken);
}