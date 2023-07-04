using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheDashboard.DashboardService.Infrastructure.Migrations
{
    public partial class Layout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dashboards_Layouts_DashboardId",
                table: "Dashboards");

            migrationBuilder.DropColumn(
                name: "DashboardId",
                table: "Layouts");

            migrationBuilder.RenameColumn(
                name: "DashboardId",
                table: "Dashboards",
                newName: "LayoutId");

            migrationBuilder.RenameIndex(
                name: "IX_Dashboards_DashboardId",
                table: "Dashboards",
                newName: "IX_Dashboards_LayoutId");

            migrationBuilder.AddColumn<int>(
                name: "LayoutId1",
                table: "Dashboards",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dashboards_LayoutId1",
                table: "Dashboards",
                column: "LayoutId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Dashboards_Layouts_LayoutId",
                table: "Dashboards",
                column: "LayoutId",
                principalTable: "Layouts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dashboards_Layouts_LayoutId1",
                table: "Dashboards",
                column: "LayoutId1",
                principalTable: "Layouts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dashboards_Layouts_LayoutId",
                table: "Dashboards");

            migrationBuilder.DropForeignKey(
                name: "FK_Dashboards_Layouts_LayoutId1",
                table: "Dashboards");

            migrationBuilder.DropIndex(
                name: "IX_Dashboards_LayoutId1",
                table: "Dashboards");

            migrationBuilder.DropColumn(
                name: "LayoutId1",
                table: "Dashboards");

            migrationBuilder.RenameColumn(
                name: "LayoutId",
                table: "Dashboards",
                newName: "DashboardId");

            migrationBuilder.RenameIndex(
                name: "IX_Dashboards_LayoutId",
                table: "Dashboards",
                newName: "IX_Dashboards_DashboardId");

            migrationBuilder.AddColumn<Guid>(
                name: "DashboardId",
                table: "Layouts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Dashboards_Layouts_DashboardId",
                table: "Dashboards",
                column: "DashboardId",
                principalTable: "Layouts",
                principalColumn: "Id");
        }
    }
}
