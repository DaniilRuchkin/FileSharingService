using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileSharingService.Migrations
{
    /// <inheritdoc />
    public partial class RenameTimeToUploadFileTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Files",
                newName: "UploadFileTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UploadFileTime",
                table: "Files",
                newName: "Time");
        }
    }
}
