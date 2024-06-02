using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecome2.Migrations
{
    /// <inheritdoc />
    public partial class changeusersendmessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSentByUser",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSentByUser",
                table: "Messages");
        }
    }
}
