using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoviesAdmin.Migrations
{
    /// <inheritdoc />
    public partial class FixUserSnapshots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSnapshots",
                table: "UserSnapshots");

            migrationBuilder.RenameTable(
                name: "UserSnapshots",
                newName: "UserSnapshot");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSnapshot",
                table: "UserSnapshot",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSnapshot",
                table: "UserSnapshot");

            migrationBuilder.RenameTable(
                name: "UserSnapshot",
                newName: "UserSnapshots");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSnapshots",
                table: "UserSnapshots",
                column: "UserId");
        }
    }
}
