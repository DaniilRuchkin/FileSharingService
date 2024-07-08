﻿namespace FileSharingService.Models;

public class EntityFile
{
    public int Id { get; set; }
    public string UniqueName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime Time { get; set; }
}