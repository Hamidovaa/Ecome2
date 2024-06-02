using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecome2.Migrations
{
    /// <inheritdoc />
    public partial class SendMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSentByUser",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "Receiver",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sender",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Receiver",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Sender",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "Messages");

            migrationBuilder.AddColumn<bool>(
                name: "IsSentByUser",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
