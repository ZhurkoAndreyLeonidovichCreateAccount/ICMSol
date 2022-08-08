using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    public partial class Init1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PayerAccountNumbers",
                columns: table => new
                {
                    Name = table.Column<int>(type: "int", nullable: false)
                        
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayerAccountNumbers", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "CheckPayerAccountNumbers",
                columns: table => new
                {
                    Name = table.Column<int>(type: "int", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckPayerAccountNumbers", x => new { x.Name, x.UserEmail });
                    table.ForeignKey(
                        name: "FK_CheckPayerAccountNumbers_Users_UserEmail",
                        column: x => x.UserEmail,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckPayerAccountNumbers_UserEmail",
                table: "CheckPayerAccountNumbers",
                column: "UserEmail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckPayerAccountNumbers");

            migrationBuilder.DropTable(
                name: "PayerAccountNumbers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
