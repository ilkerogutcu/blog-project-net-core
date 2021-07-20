using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.DataAccess.Migrations
{
    public partial class CreateSeoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "seo_author",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "seo_description",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "seo_tags",
                table: "posts");

            migrationBuilder.AddColumn<Guid>(
                name: "seo_detail_id",
                table: "posts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "seo_details",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    seo_author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    seo_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    seo_tags = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_seo_details", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_posts_seo_detail_id",
                table: "posts",
                column: "seo_detail_id");

            migrationBuilder.AddForeignKey(
                name: "fk_posts_seo_details_seo_detail_id",
                table: "posts",
                column: "seo_detail_id",
                principalTable: "seo_details",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_posts_seo_details_seo_detail_id",
                table: "posts");

            migrationBuilder.DropTable(
                name: "seo_details");

            migrationBuilder.DropIndex(
                name: "ix_posts_seo_detail_id",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "seo_detail_id",
                table: "posts");

            migrationBuilder.AddColumn<string>(
                name: "seo_author",
                table: "posts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "seo_description",
                table: "posts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "seo_tags",
                table: "posts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
