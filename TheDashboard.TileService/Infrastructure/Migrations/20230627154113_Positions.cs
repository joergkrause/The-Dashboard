using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheDashboard.TileService.Infrastructure.Migrations
{
    public partial class Positions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tiles_Vizalizers_VisualizerId",
                table: "Tiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Vizalizers_Transformers_TransformerId",
                table: "Vizalizers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vizalizers",
                table: "Vizalizers");

            migrationBuilder.RenameTable(
                name: "Vizalizers",
                newName: "Visualizers");

            migrationBuilder.RenameIndex(
                name: "IX_Vizalizers_TransformerId",
                table: "Visualizers",
                newName: "IX_Visualizers_TransformerId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Transformers",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512);

            migrationBuilder.AddColumn<int>(
                name: "Position_Height",
                table: "Tiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Position_Width",
                table: "Tiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Position_XOffset",
                table: "Tiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Position_YOffset",
                table: "Tiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Visualizers",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Visualizers",
                table: "Visualizers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "InboxState",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConsumerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LockId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Received = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReceiveCount = table.Column<int>(type: "int", nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Consumed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Delivered = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastSequenceNumber = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboxState", x => x.Id);
                    table.UniqueConstraint("AK_InboxState_MessageId_ConsumerId", x => new { x.MessageId, x.ConsumerId });
                });

            migrationBuilder.CreateIndex(
                name: "IX_InboxState_Delivered",
                table: "InboxState",
                column: "Delivered");

            migrationBuilder.AddForeignKey(
                name: "FK_Tiles_Visualizers_VisualizerId",
                table: "Tiles",
                column: "VisualizerId",
                principalTable: "Visualizers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Visualizers_Transformers_TransformerId",
                table: "Visualizers",
                column: "TransformerId",
                principalTable: "Transformers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tiles_Visualizers_VisualizerId",
                table: "Tiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Visualizers_Transformers_TransformerId",
                table: "Visualizers");

            migrationBuilder.DropTable(
                name: "InboxState");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Visualizers",
                table: "Visualizers");

            migrationBuilder.DropColumn(
                name: "Position_Height",
                table: "Tiles");

            migrationBuilder.DropColumn(
                name: "Position_Width",
                table: "Tiles");

            migrationBuilder.DropColumn(
                name: "Position_XOffset",
                table: "Tiles");

            migrationBuilder.DropColumn(
                name: "Position_YOffset",
                table: "Tiles");

            migrationBuilder.RenameTable(
                name: "Visualizers",
                newName: "Vizalizers");

            migrationBuilder.RenameIndex(
                name: "IX_Visualizers_TransformerId",
                table: "Vizalizers",
                newName: "IX_Vizalizers_TransformerId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Transformers",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Vizalizers",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vizalizers",
                table: "Vizalizers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tiles_Vizalizers_VisualizerId",
                table: "Tiles",
                column: "VisualizerId",
                principalTable: "Vizalizers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Vizalizers_Transformers_TransformerId",
                table: "Vizalizers",
                column: "TransformerId",
                principalTable: "Transformers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
