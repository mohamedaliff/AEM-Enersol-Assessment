using Microsoft.EntityFrameworkCore.Migrations;

namespace AEM_Enersol_Assessment.Migrations
{
    public partial class TurnOffIdentityInsert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Platform",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    UniqueName = table.Column<string>(nullable: true),
                    Latitude = table.Column<float>(nullable: false),
                    Longitude = table.Column<float>(nullable: false),
                    CreatedAt = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platform", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Well",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    PlatformID = table.Column<int>(nullable: false),
                    UniqueName = table.Column<string>(nullable: true),
                    Latitude = table.Column<float>(nullable: false),
                    Longitude = table.Column<float>(nullable: false),
                    CreatedAt = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Well", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Well_Platform_PlatformID",
                        column: x => x.PlatformID,
                        principalTable: "Platform",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Well_PlatformID",
                table: "Well",
                column: "PlatformID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Well");

            migrationBuilder.DropTable(
                name: "Platform");
        }
    }
}
