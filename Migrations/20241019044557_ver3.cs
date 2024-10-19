using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crickinfo_mvc_ef_core.Migrations
{
    /// <inheritdoc />
    public partial class ver3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamTournament_Teams_TeamsTeamId",
                table: "TeamTournament");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamTournament_Tournaments_TournamentsId",
                table: "TeamTournament");

            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Users_UserId",
                table: "Tournaments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamTournament",
                table: "TeamTournament");

            migrationBuilder.RenameTable(
                name: "TeamTournament",
                newName: "TeamTournaments");

            migrationBuilder.RenameColumn(
                name: "TournamentsId",
                table: "TeamTournaments",
                newName: "TournamentId");

            migrationBuilder.RenameColumn(
                name: "TeamsTeamId",
                table: "TeamTournaments",
                newName: "TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_TeamTournament_TournamentsId",
                table: "TeamTournaments",
                newName: "IX_TeamTournaments_TournamentId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Tournaments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TeamTournaments",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateJoined",
                table: "TeamTournaments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeamStatus",
                table: "TeamTournaments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamTournaments",
                table: "TeamTournaments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TeamTournament1",
                columns: table => new
                {
                    TeamsTeamId = table.Column<int>(type: "int", nullable: false),
                    TournamentsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamTournament1", x => new { x.TeamsTeamId, x.TournamentsId });
                    table.ForeignKey(
                        name: "FK_TeamTournament1_Teams_TeamsTeamId",
                        column: x => x.TeamsTeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamTournament1_Tournaments_TournamentsId",
                        column: x => x.TournamentsId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamTournaments_TeamId",
                table: "TeamTournaments",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamTournament1_TournamentsId",
                table: "TeamTournament1",
                column: "TournamentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamTournaments_Teams_TeamId",
                table: "TeamTournaments",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamTournaments_Tournaments_TournamentId",
                table: "TeamTournaments",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Users_UserId",
                table: "Tournaments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamTournaments_Teams_TeamId",
                table: "TeamTournaments");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamTournaments_Tournaments_TournamentId",
                table: "TeamTournaments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Users_UserId",
                table: "Tournaments");

            migrationBuilder.DropTable(
                name: "TeamTournament1");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamTournaments",
                table: "TeamTournaments");

            migrationBuilder.DropIndex(
                name: "IX_TeamTournaments_TeamId",
                table: "TeamTournaments");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TeamTournaments");

            migrationBuilder.DropColumn(
                name: "DateJoined",
                table: "TeamTournaments");

            migrationBuilder.DropColumn(
                name: "TeamStatus",
                table: "TeamTournaments");

            migrationBuilder.RenameTable(
                name: "TeamTournaments",
                newName: "TeamTournament");

            migrationBuilder.RenameColumn(
                name: "TournamentId",
                table: "TeamTournament",
                newName: "TournamentsId");

            migrationBuilder.RenameColumn(
                name: "TeamId",
                table: "TeamTournament",
                newName: "TeamsTeamId");

            migrationBuilder.RenameIndex(
                name: "IX_TeamTournaments_TournamentId",
                table: "TeamTournament",
                newName: "IX_TeamTournament_TournamentsId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Tournaments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamTournament",
                table: "TeamTournament",
                columns: new[] { "TeamsTeamId", "TournamentsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TeamTournament_Teams_TeamsTeamId",
                table: "TeamTournament",
                column: "TeamsTeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamTournament_Tournaments_TournamentsId",
                table: "TeamTournament",
                column: "TournamentsId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Users_UserId",
                table: "Tournaments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
