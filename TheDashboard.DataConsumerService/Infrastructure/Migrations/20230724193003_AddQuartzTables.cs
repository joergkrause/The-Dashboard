using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheDashboard.DataConsumerService.Infrastructure.Migrations
{
  public partial class AddQuartzTables : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      string sql = File.ReadAllText("Infrastructure/Migrations/QuartzSql/sqlserver.sql");

      // get name of database here 
      var dbName = "DataConsumerServiceDb";

      migrationBuilder.Sql(sql.Replace("<DB_NAME>", dbName));
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      throw new NotImplementedException("Removing quartz is currently not supported");
    }
  }
}
