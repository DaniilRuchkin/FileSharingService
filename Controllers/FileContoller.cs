using Microsoft.AspNetCore.Mvc;
using FileSharingService.DTO;
using System.ComponentModel.DataAnnotations;
using FileSharingService.Filters;
using FileSharingService.Services;

namespace FileSharingService.Controllers;

[Route("api/files")]
[ApiController]
[TypeFilter(typeof(NullCheckExceptionFilter))]
public class FileContoller(IFilesServices services, IWebHostEnvironment webHostEnvironment) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateFileAsync([FromForm] CreateFileDto createFileDto)
    {
        var filePath = await services.FileSaveAsync(createFileDto);

        return Ok(filePath);
    }

    [HttpGet("{uniqueFileName}")]
    public async Task<IActionResult> GetFileAsync(string uniqueFileName)
    {
        var dowloadedFile = await services.DowloadFileAsync(uniqueFileName);
        var webRoot = webHostEnvironment.WebRootPath;
        var filePath = Path.Combine(webRoot, dowloadedFile.UniqueName);

        return PhysicalFile(filePath, "application/octet-stream", Path.GetFileName(filePath));
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteFileAsync(FileDeleteDto fileDeleteDto)
    {
        var fileToDelete = await services.DeleteFileAsync(fileDeleteDto);
        
        return Ok(fileToDelete);
    }
}
