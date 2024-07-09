using FileSharingService.DTO;
using FileSharingService.Filters;
using FileSharingService.Response;
using FileSharingService.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileSharingService.Controllers;

[Route("api/v1/files")]
[ApiController]
[TypeFilter(typeof(NullCheckExceptionFilter))]
public class FileContoller(IFileService services, IWebHostEnvironment webHostEnvironment) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateFileAsync([FromForm] CreateFileDto createFileDto)
    {
        var fileData = await services.FileSaveAsync(createFileDto);

        var response = new Response<FileDataDto>
        {
            Data = fileData
        };

        return Ok(response);
    }

    [HttpGet("{uniqueFileName}")]
    public async Task<IActionResult> GetFileAsync([FromRoute] string uniqueFileName)
    {
        var dowloadedFile = await services.DowloadFileAsync(uniqueFileName);
        var webRoot = webHostEnvironment.WebRootPath;
        var filePath = Path.Combine(webRoot, dowloadedFile.UniqueName);

        return PhysicalFile(filePath, "application/octet-stream", Path.GetFileName(filePath));
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteFileAsync(FileDeleteDto fileDeleteDto)
    {
        var fileDataToDelete = await services.DeleteFileAsync(fileDeleteDto);

        var response = new Response<FileDataDto>
        {
            Data = fileDataToDelete
        };

        return Ok(response);
    }
}