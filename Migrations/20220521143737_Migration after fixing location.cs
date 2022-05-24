using Microsoft.EntityFrameworkCore.Migrations;

namespace LetsTalkBackend.Migrations
{
    public partial class Migrationafterfixinglocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_locations_LocationIdLocation",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_locations",
                table: "locations");

            migrationBuilder.RenameTable(
                name: "locations",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "LocationIdLocation",
                table: "users",
                newName: "LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_users_LocationIdLocation",
                table: "users",
                newName: "IX_users_LocationId");

            migrationBuilder.RenameColumn(
                name: "IdLocation",
                table: "Location",
                newName: "Id");

            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Location",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Location",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Location",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "Location",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Location",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Location",
                table: "Location",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_users_Location_LocationId",
                table: "users",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_Location_LocationId",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Location",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Location");

            migrationBuilder.RenameTable(
                name: "Location",
                newName: "locations");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "users",
                newName: "LocationIdLocation");

            migrationBuilder.RenameIndex(
                name: "IX_users_LocationId",
                table: "users",
                newName: "IX_users_LocationIdLocation");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "locations",
                newName: "IdLocation");

            migrationBuilder.AlterColumn<int>(
                name: "Longitude",
                table: "locations",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "Latitude",
                table: "locations",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddPrimaryKey(
                name: "PK_locations",
                table: "locations",
                column: "IdLocation");

            migrationBuilder.AddForeignKey(
                name: "FK_users_locations_LocationIdLocation",
                table: "users",
                column: "LocationIdLocation",
                principalTable: "locations",
                principalColumn: "IdLocation",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
