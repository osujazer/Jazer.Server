using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jazer.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddUserStatisticsEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_peak_ranks",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    rank = table.Column<int>(type: "integer", nullable: false),
                    achieved_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_peak_ranks", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_user_peak_ranks_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_statistics",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    jazer_points = table.Column<int>(type: "integer", nullable: false),
                    ranked_score = table.Column<long>(type: "bigint", nullable: false),
                    total_score = table.Column<long>(type: "bigint", nullable: false),
                    average_accuracy = table.Column<double>(type: "double precision", nullable: false),
                    global_rank = table.Column<int>(type: "integer", nullable: true),
                    country_rank = table.Column<int>(type: "integer", nullable: true),
                    play_time = table.Column<long>(type: "bigint", nullable: false),
                    play_count = table.Column<int>(type: "integer", nullable: false),
                    max_combo = table.Column<int>(type: "integer", nullable: false),
                    total_hits = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_statistics", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_user_statistics_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_peak_ranks");

            migrationBuilder.DropTable(
                name: "user_statistics");
        }
    }
}
