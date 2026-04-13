using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TrainerMemberId",
                table: "TrainingSession",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSession_TrainerMemberId",
                table: "TrainingSession",
                column: "TrainerMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingSession_Members_TrainerMemberId",
                table: "TrainingSession",
                column: "TrainerMemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingSession_Members_TrainerMemberId",
                table: "TrainingSession");

            migrationBuilder.DropIndex(
                name: "IX_TrainingSession_TrainerMemberId",
                table: "TrainingSession");

            migrationBuilder.DropColumn(
                name: "TrainerMemberId",
                table: "TrainingSession");
        }
    }
}
