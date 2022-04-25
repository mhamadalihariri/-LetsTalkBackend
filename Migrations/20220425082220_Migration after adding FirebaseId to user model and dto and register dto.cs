using Microsoft.EntityFrameworkCore.Migrations;

namespace LetsTalkBackend.Migrations
{
    public partial class MigrationafteraddingFirebaseIdtousermodelanddtoandregisterdto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirebaseId",
                table: "users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirebaseId",
                table: "users");
        }
    }
}
