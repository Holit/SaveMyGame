using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaveMyGame.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationConfigs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FromPath = table.Column<string>(type: "TEXT", nullable: true),
                    ToPath = table.Column<string>(type: "TEXT", nullable: true),
                    Interval = table.Column<int>(type: "INTEGER", nullable: false),
                    IsFastMode = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsUsingLZMA = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsRemindedSize = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsDeleteOldFiles = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsClearBeforeRestore = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileRecords",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FilePath = table.Column<string>(type: "TEXT", nullable: false),
                    RestorePath = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Size = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileRecords", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationConfigs");

            migrationBuilder.DropTable(
                name: "FileRecords");
        }
    }
}
