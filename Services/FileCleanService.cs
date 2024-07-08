﻿using FileSharingService.Repository;
using Microsoft.Extensions.Options;

namespace FileSharingService.FileClean;

public class FileCleanService(IServiceProvider serviceProvider, IConfiguration configuration) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        int cleanIntervalDays = configuration.GetValue<int>("CleanupSettings:CleanupIntervalDays");

        while (!cancellationToken.IsCancellationRequested)
        {
            await CleanupOldFilesAsync();

            var cleanupInterval = TimeSpan.FromDays(cleanIntervalDays);

            await Task.Delay(cleanupInterval, cancellationToken);
        }
    }

    private async Task CleanupOldFilesAsync()
    {
        int deleteBeforeDays = configuration.GetValue<int>("CleanupSettings:DeleteBeforeDays");
        var deleteBefore = DateTime.UtcNow.AddDays(-deleteBeforeDays);

        using (var scope = serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IFileRepository>();
            var filesToDelete = await repository.GetFilesToDeleteAsync(deleteBefore);

            foreach (var file in filesToDelete)
            {
                if (File.Exists(file.FilePath))
                {
                    File.Delete(file.FilePath);
                }

                await repository.DeleteFileAsync(file);
            }
            await repository.SaveShangesAsync();
        }
    }
}