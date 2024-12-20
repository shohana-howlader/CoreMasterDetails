using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreMasterDetails.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PerformanceReviews",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReviewNotes = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PerformanceReviews__C92D71A707928021", x => x.ReviewId);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeName = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    JoiningDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MobileNo = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    ImageURL = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ReviewId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Employees__32C52B999596CBD4", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK__Employees__PerformanceReview__267ABA7A",
                        column: x => x.ReviewId,
                        principalTable: "PerformanceReviews",
                        principalColumn: "ReviewId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Budget = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Departments__2B7477A7F6E4BE3F", x => x.DepartmentId);
                    table.ForeignKey(
                        name: "FK__Departments__Employees__29572725",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_EmployeeId",
                table: "Departments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ReviewId",
                table: "Employees",
                column: "ReviewId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "PerformanceReviews");
        }
    }
}
