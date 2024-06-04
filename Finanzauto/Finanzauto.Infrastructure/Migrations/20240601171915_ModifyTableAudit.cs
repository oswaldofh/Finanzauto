using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finanzauto.Infrastructure.Migrations
{
    public partial class ModifyTableAudit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Document",
                table: "VehicleAudits");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Document",
                table: "VehicleAudits",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");
        }
    }
}
