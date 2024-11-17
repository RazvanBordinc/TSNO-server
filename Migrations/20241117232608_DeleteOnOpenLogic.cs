using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSNO.Migrations
{
    /// <inheritdoc />
    public partial class DeleteOnOpenLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DeleteWhenOpen",
                table: "Entities",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteWhenOpen",
                table: "Entities");
        }
    }
}
