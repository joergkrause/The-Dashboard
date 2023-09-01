using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheDashboard.DataSourceService.Infrastructure.Migrations
{
    public partial class OutboxVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MessageType",
                table: "OutboxMessage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageType",
                table: "OutboxMessage");
        }
    }
}
