using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace C43COOL.MySql.Migrations
{
    public partial class addCarousel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carousel",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    Title = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    FileId = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Link = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateModify = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carousel", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Carousel");
        }
    }
}
