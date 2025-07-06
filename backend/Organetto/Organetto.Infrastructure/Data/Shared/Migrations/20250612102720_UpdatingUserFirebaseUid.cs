using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Organetto.Infrastructure.Data.Shared.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingUserFirebaseUid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "firebase_uid",
                table: "user",
                type: "character varying(36)",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "firebase_uid",
                table: "user",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(36)",
                oldMaxLength: 36);
        }
    }
}
