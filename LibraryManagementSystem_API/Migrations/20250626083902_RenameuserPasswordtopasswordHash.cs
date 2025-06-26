using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem_API.Migrations
{
    /// <inheritdoc />
    public partial class RenameuserPasswordtopasswordHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "userPassword",
                table: "Users",
                newName: "passwordHash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "passwordHash",
                table: "Users",
                newName: "userPassword");
        }
    }
}
