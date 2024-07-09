using FileSharingService.DTO;
using FileSharingService.Models;
using FileSharingService.Repository;
using Microsoft.AspNetCore.Identity;
using Visus.Cuid;

namespace FileSharingService.Services;

public class FileService(IWebHostEnvironment webHostEnvironment, IFileRepository repository, IPasswordHasher<string> passwordHasher) : IFileService
{
    public async Task<FileDataDto> DeleteFileAsync(FileDeleteDto fileDeleteDto, CancellationToken cancellationToken)
    {
        var fileToDelete = await repository.GetFileAsync(fileDeleteDto.UniqueFileName, cancellationToken);

        if (fileToDelete == null)
        {
            return new FileDataDto { IsSuccess = false };
        }

        var passwordVerificationPassword = passwordHasher.VerifyHashedPassword(null!, fileToDelete.Password, fileDeleteDto.Password);

        if (passwordVerificationPassword == PasswordVerificationResult.Success)
        {
            File.Delete(fileToDelete.FilePath);

            await repository.DeleteFileAsync(fileToDelete, cancellationToken);

            return new FileDataDto { IsSuccess = true };
        }

        return new FileDataDto { IsSuccess = false };
    }

    public async Task<FileDataDto> DowloadFileAsync(string fileName, CancellationToken cancellationToken)
    {
        var getFile = await repository.GetFileAsync(fileName, cancellationToken);
        
        if (getFile == null)
        {
            throw new NullReferenceException("File not found.");
        
        }

        var webRoot = webHostEnvironment.WebRootPath;
        var filePath = Path.Combine(webRoot, getFile.UniqueName);

        return new FileDataDto { IsSuccess = true, FilePath = filePath };
    }

    public async Task<FileDataDto> FileSaveAsync(CreateFileDto dtoFile, CancellationToken cancellationToken)
    {
        var uniqueFileName = new Cuid2() + Path.GetExtension(dtoFile.File.FileName);
        var filePath = Path.Combine(webHostEnvironment.WebRootPath, uniqueFileName);

        await using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize:4096, useAsync: true))
        {
            await dtoFile.File.CopyToAsync(stream);
        }

        var hashedPassword = passwordHasher.HashPassword(null!, dtoFile.Password);

        var newFile = new Document
        {
            UniqueName = uniqueFileName,
            FilePath = filePath,
            Password = hashedPassword,
            UploadFileTime = DateTime.UtcNow,
        };

        await repository.SaveFileAsync(newFile, cancellationToken);

        return new FileDataDto { FilePath = filePath, IsSuccess = true };
    }
}