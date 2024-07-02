using FileSharingService.Repository;

namespace FileSharingService.FileClean;

public class FilesCleanServices(IServiceProvider serviceProvider, IConfiguration configuration) : BackgroundService
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
            var repository = scope.ServiceProvider.GetRequiredService<IRepository>();

            await repository.DeleteFileTimeAsync(deleteBefore);
        }
    }
}


