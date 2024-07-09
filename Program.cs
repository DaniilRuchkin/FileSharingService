using FileSharingService.DbContextFile;
using Microsoft.EntityFrameworkCore;
using FileSharingService.Repository;
using FileSharingService.FileClean;
using FileSharingService.Services;
using FileSharingService.Filters;
using FileSharingService.Configurations;
using Microsoft.AspNetCore.Identity;
using FileSharingService.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

DotNetEnv.Env.Load("./.env");
var connection = Environment.GetEnvironmentVariables();
var connectionString = $"""
                        Host={connection["DATABASE_HOST"]}; 
                        Port={connection["DATABASE_PORT"]}; 
                        Database={connection["DATABASE_NAME"]}; 
                        Username={connection["DATABASE_USER"]}; 
                        Password={connection["DATABASE_PASSWORD"]};
                        """;

builder.Services.AddDbContext<DocumentDbContext>(options =>
options.UseNpgsql(connectionString));

builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IPasswordHasher<string>, PasswordHasher<string>>();

builder.Services.Configure<CleanSettings>(builder.Configuration.GetSection("CleanupSettings"));

builder.Services.AddHostedService<FileCleanService>();

builder.Services.AddControllers(op =>
{
    op.Filters.Add<NullCheckExceptionFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorMiddleware>();

app.UseStaticFiles();
app.UseRouting();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();