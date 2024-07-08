using FileSharingService.DTO;
using FileSharingService.Models;
using Visus.Cuid;
using FileSharingService.Repository;
using Microsoft.AspNetCore.Identity;

namespace FileSharingService.Services;

public class FileService(IWebHostEnvironment webHostEnvironment, IFileRepository repository, IPasswordHasher<string> passwordHasher) : IFileService
{
    public async Task<bool> DeleteFileAsync(FileDeleteDto fileDeleteDto)
    {
        var fileToDelete = await repository.GetFileAsync(fileDeleteDto.UniqueFileName);

        if (fileToDelete == null) return false;

        var passwordVerificationPassword = passwordHasher.VerifyHashedPassword(null!, fileToDelete.Password, fileDeleteDto.Password);

        if (passwordVerificationPassword == PasswordVerificationResult.Success)
        {
            File.Delete(fileToDelete.FilePath);

            await repository.DeleteFileAsync(fileToDelete);

            return true;
        }

        return false;
    }

    public async Task<Document> DowloadFileAsync(string fileName)
    {
        return await repository.GetFileAsync(fileName);
    }

    public async Task<string> FileSaveAsync(CreateFileDto dtoFile)
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

        await repository.SaveFileAsync(newFile);

        return uniqueFileName;
    }
}