using FileSharingService.DbContextFile;
using Microsoft.EntityFrameworkCore;
using FileSharingService.Repository;
using FileSharingService.FileClean;
using FileSharingService.Services;
using FileSharingService.Filters;

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

builder.Services.AddDbContext<ManagementDbContext>(options =>
options.UseNpgsql(connectionString));

builder.Services.AddScoped<IFilesServices, FilesServices>();
builder.Services.AddScoped<IRepository, Repository>();  
builder.Services.AddHostedService<FilesCleanServices>();

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

app.UseStaticFiles();
app.UseRouting();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();