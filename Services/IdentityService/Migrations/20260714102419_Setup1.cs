using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityService.Migrations
{
    /// <inheritdoc />
    public partial class Setup1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "DealsUsers");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                table: "DealsUsers",
                newName: "IX_DealsUsers_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DealsUsers",
                table: "DealsUsers",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DealsUsers",
                table: "DealsUsers");

            migrationBuilder.RenameTable(
                name: "DealsUsers",
                newName: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_DealsUsers_Email",
                table: "Users",
                newName: "IX_Users_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");
        }
    }
}
