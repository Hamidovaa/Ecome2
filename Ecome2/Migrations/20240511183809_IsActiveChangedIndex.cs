using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecome2.Migrations
{
    /// <inheritdoc />
    public partial class IsActiveChangedIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsCheck",
                table: "Products",
                newName: "IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Products",
                newName: "IsCheck");
        }
    }
}
