using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_MembershipPlan_MembershipPlansId",
                table: "Memberships");

            migrationBuilder.RenameColumn(
                name: "MembershipPlansId",
                table: "Memberships",
                newName: "MembershipPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_Memberships_MembershipPlansId",
                table: "Memberships",
                newName: "IX_Memberships_MembershipPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_MemberId",
                table: "Memberships",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_Members_MemberId",
                table: "Memberships",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_MembershipPlan_MembershipPlanId",
                table: "Memberships",
                column: "MembershipPlanId",
                principalTable: "MembershipPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_Members_MemberId",
                table: "Memberships");

            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_MembershipPlan_MembershipPlanId",
                table: "Memberships");

            migrationBuilder.DropIndex(
                name: "IX_Memberships_MemberId",
                table: "Memberships");

            migrationBuilder.RenameColumn(
                name: "MembershipPlanId",
                table: "Memberships",
                newName: "MembershipPlansId");

            migrationBuilder.RenameIndex(
                name: "IX_Memberships_MembershipPlanId",
                table: "Memberships",
                newName: "IX_Memberships_MembershipPlansId");

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_MembershipPlan_MembershipPlansId",
                table: "Memberships",
                column: "MembershipPlansId",
                principalTable: "MembershipPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
