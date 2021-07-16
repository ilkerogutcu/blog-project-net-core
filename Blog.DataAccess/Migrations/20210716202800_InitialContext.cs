using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.DataAccess.Migrations
{
    public partial class InitialContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "application_user_notification");

            migrationBuilder.CreateTable(
                name: "notification_user",
                columns: table => new
                {
                    notifications_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    users_id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notification_user", x => new { x.notifications_id, x.users_id });
                    table.ForeignKey(
                        name: "fk_notification_user_notifications_notifications_id",
                        column: x => x.notifications_id,
                        principalTable: "notifications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_notification_user_users_users_id",
                        column: x => x.users_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_notification_user_users_id",
                table: "notification_user",
                column: "users_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "notification_user");

            migrationBuilder.CreateTable(
                name: "application_user_notification",
                columns: table => new
                {
                    notifications_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    users_id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_application_user_notification", x => new { x.notifications_id, x.users_id });
                    table.ForeignKey(
                        name: "fk_application_user_notification_notifications_notifications_id",
                        column: x => x.notifications_id,
                        principalTable: "notifications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_application_user_notification_users_users_id",
                        column: x => x.users_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_application_user_notification_users_id",
                table: "application_user_notification",
                column: "users_id");
        }
    }
}
