using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class removeforeignkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddTasks_Users_AssigneeId",
                table: "AddTasks");

            migrationBuilder.DropIndex(
                name: "IX_AddTasks_AssigneeId",
                table: "AddTasks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AddTasks_AssigneeId",
                table: "AddTasks",
                column: "AssigneeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AddTasks_Users_AssigneeId",
                table: "AddTasks",
                column: "AssigneeId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
