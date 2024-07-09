using FileSharingService.Configurations;
using FileSharingService.Repository;
using Microsoft.Extensions.Options;

namespace FileSharingService.FileClean;

public class FileCleanService(IServiceProvider serviceProvider, IOptions<CleanSettings> options) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var cleanIntervalDays = options.Value.CleanupIntervalDays;
        
        while (!cancellationToken.IsCancellationRequested)
        {
            await CleanupOldFilesAsync(cancellationToken);

            var cleanupInterval = TimeSpan.FromDays(cleanIntervalDays);

            await Task.Delay(cleanupInterval, cancellationToken);
        }
    }

    private async Task CleanupOldFilesAsync(CancellationToken cancellationToken)
    {
        var deleteBeforeDays = options.Value.DeleteBeforeDays;
        var deleteBefore = DateTime.UtcNow.AddDays(-deleteBeforeDays);

        using (var scope = serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IFileRepository>();
            var filesToDelete = await repository.GetFilesToDeleteAsync(deleteBefore, cancellationToken);

            foreach (var file in filesToDelete)
            {
                if (File.Exists(file.FilePath))
                {
                    File.Delete(file.FilePath);
                }

                await repository.DeleteFileAsync(file, cancellationToken);
            }
            await repository.SaveShangesAsync(cancellationToken);
        }
    }
}