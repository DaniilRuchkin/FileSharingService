using System.ComponentModel.DataAnnotations;

namespace FileSharingService.DTO;

public class FileDeleteDto
{
    [Required, MinLength(3), MaxLength(64)]
    public string? UniqueFileName { get; set; }

    [Required, MinLength(3), MaxLength(64)]
    public string? Password { get; set; }
}