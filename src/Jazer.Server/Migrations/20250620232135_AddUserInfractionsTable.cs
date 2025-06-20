using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jazer.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddUserInfractionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_infractions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    expires_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    assigned_by_user_id = table.Column<int>(type: "integer", nullable: false),
                    infraction_type = table.Column<byte>(type: "smallint", nullable: false),
                    infraction_reason = table.Column<byte>(type: "smallint", nullable: false),
                    public_detail = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    internal_detail = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_infractions", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_infractions_users_assigned_by_user_id",
                        column: x => x.assigned_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_infractions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_infractions_assigned_by_user_id",
                table: "user_infractions",
                column: "assigned_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_infractions_user_id",
                table: "user_infractions",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_infractions");
        }
    }
}
