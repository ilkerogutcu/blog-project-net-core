using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.DataAccess.Migrations
{
    public partial class UpdatedPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_posts_categories_category_id",
                table: "posts");

            migrationBuilder.DropIndex(
                name: "ix_posts_category_id",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "category_id",
                table: "posts");

            migrationBuilder.CreateTable(
                name: "category_post",
                columns: table => new
                {
                    categories_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    posts_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_category_post", x => new { x.categories_id, x.posts_id });
                    table.ForeignKey(
                        name: "fk_category_post_categories_categories_id",
                        column: x => x.categories_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_category_post_posts_posts_id",
                        column: x => x.posts_id,
                        principalTable: "posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_category_post_posts_id",
                table: "category_post",
                column: "posts_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "category_post");

            migrationBuilder.AddColumn<Guid>(
                name: "category_id",
                table: "posts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_posts_category_id",
                table: "posts",
                column: "category_id");

            migrationBuilder.AddForeignKey(
                name: "fk_posts_categories_category_id",
                table: "posts",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
