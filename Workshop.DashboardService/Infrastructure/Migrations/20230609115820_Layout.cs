using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workshop.DashboardService.Migrations
{
    public partial class Layout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LayoutType",
                table: "Layouts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidFrom",
                table: "Layouts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidTo",
                table: "Layouts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Settings_PropertyBag",
                table: "Dashboards",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LayoutType",
                table: "Layouts");

            migrationBuilder.DropColumn(
                name: "ValidFrom",
                table: "Layouts");

            migrationBuilder.DropColumn(
                name: "ValidTo",
                table: "Layouts");

            migrationBuilder.DropColumn(
                name: "Settings_PropertyBag",
                table: "Dashboards");
        }
    }
}
