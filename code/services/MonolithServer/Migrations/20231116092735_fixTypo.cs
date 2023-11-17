using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonolithServer.Migrations
{
    public partial class fixTypo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContantInfoId",
                table: "Orders",
                newName: "ContactInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContactInfoId",
                table: "Orders",
                newName: "ContantInfoId");
        }
    }
}
