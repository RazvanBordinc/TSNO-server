using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSNO.Migrations
{
    /// <inheritdoc />
    public partial class CountingSystemModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalNotes = table.Column<int>(type: "int", nullable: false),
                    ActiveNotes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stats", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Stats",
                columns: new[] { "Id", "ActiveNotes", "TotalNotes" },
                values: new object[] { 1, 0, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stats");
        }
    }
}
