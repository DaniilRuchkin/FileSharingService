using System.ComponentModel.DataAnnotations;

namespace FileSharingService.DTO;

public class CreateFileDto
{
    [Required]
    public IFormFile File { get; set; } = null!;

    [Required, MinLength(3), MaxLength(64)]
    public string Password { get; set; } = string.Empty;
}