using FileSharingService.DTO;
using FileSharingService.Filters;
using FileSharingService.Response;
using FileSharingService.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileSharingService.Controllers;

[Route("api/v1/files")]
[ApiController]
[TypeFilter(typeof(NullCheckExceptionFilter))]
public class FileContoller(IFileService services) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateFileAsync([FromForm] CreateFileDto createFileDto, CancellationToken cancellationToken)
    {
        var fileData = await services.FileSaveAsync(createFileDto, cancellationToken);

        var response = new BaseResponse<FileDataDto>
        {
            Data = fileData
        };

        return Ok(response);
    }

    [HttpGet("{uniqueFileName}")]
    public async Task<IActionResult> GetFileAsync([FromRoute] string uniqueFileName, CancellationToken cancellationToken)
    {
        var getDowloadedFile = await services.DowloadFileAsync(uniqueFileName, cancellationToken);
        
        return PhysicalFile(getDowloadedFile.FilePath!, "application/octet-stream", Path.GetFileName(getDowloadedFile.FilePath));
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteFileAsync(FileDeleteDto fileDeleteDto, CancellationToken cancellationToken)
    {
        var fileDataToDelete = await services.DeleteFileAsync(fileDeleteDto, cancellationToken);

        var response = new BaseResponse<FileDataDto>
        {
            Data = fileDataToDelete
        };

        return Ok(response);
    }
}