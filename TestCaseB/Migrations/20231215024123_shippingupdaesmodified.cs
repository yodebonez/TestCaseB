using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestCaseB.Migrations
{
    public partial class shippingupdaesmodified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CurrentLocation",
                table: "shipingUpdates",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ShipingIdentity",
                table: "shipingUpdates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShipingIdentity",
                table: "shipingUpdates");

            migrationBuilder.AlterColumn<string>(
                name: "CurrentLocation",
                table: "shipingUpdates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
