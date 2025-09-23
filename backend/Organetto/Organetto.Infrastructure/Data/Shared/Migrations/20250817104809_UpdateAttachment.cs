using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Organetto.Infrastructure.Data.Shared.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAttachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_attachment_card_card_id",
                table: "attachment");

            migrationBuilder.DropForeignKey(
                name: "fk_attachment_user_uploader_id",
                table: "attachment");

            migrationBuilder.DropIndex(
                name: "ix_attachment_uploader_id",
                table: "attachment");

            migrationBuilder.DropColumn(
                name: "file_url",
                table: "attachment");

            migrationBuilder.RenameColumn(
                name: "filename",
                table: "attachment",
                newName: "file_name");

            migrationBuilder.RenameColumn(
                name: "uploader_id",
                table: "attachment",
                newName: "size_bytes");

            migrationBuilder.RenameColumn(
                name: "uploaded_at",
                table: "attachment",
                newName: "created_at");

            migrationBuilder.AlterColumn<long>(
                name: "card_id",
                table: "attachment",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "checksum_sha256",
                table: "attachment",
                type: "character varying(88)",
                maxLength: 88,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "content_type",
                table: "attachment",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_at",
                table: "attachment",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "file_key",
                table: "attachment",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "metadata_json",
                table: "attachment",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{}'::jsonb");

            migrationBuilder.AddColumn<long>(
                name: "owner_user_id",
                table: "attachment",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "attachment",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "updated_at",
                table: "attachment",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "version",
                table: "attachment",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "attachment_link",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    attachment_id = table.Column<long>(type: "bigint", nullable: false),
                    owner_kind = table.Column<int>(type: "integer", nullable: false),
                    owner_id = table.Column<long>(type: "bigint", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    created_by_user_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_attachment_link", x => x.id);
                    table.ForeignKey(
                        name: "fk_attachment_link_attachments_attachment_id",
                        column: x => x.attachment_id,
                        principalTable: "attachment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_attachment_link_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_attachment_file_key",
                table: "attachment",
                column: "file_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_attachment_owner_user_id",
                table: "attachment",
                column: "owner_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_attachment_link_attachment_id_owner_kind_owner_id",
                table: "attachment_link",
                columns: new[] { "attachment_id", "owner_kind", "owner_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_attachment_link_created_by_user_id",
                table: "attachment_link",
                column: "created_by_user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_attachment_card_card_id",
                table: "attachment",
                column: "card_id",
                principalTable: "card",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_attachment_user_owner_user_id",
                table: "attachment",
                column: "owner_user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_attachment_card_card_id",
                table: "attachment");

            migrationBuilder.DropForeignKey(
                name: "fk_attachment_user_owner_user_id",
                table: "attachment");

            migrationBuilder.DropTable(
                name: "attachment_link");

            migrationBuilder.DropIndex(
                name: "ix_attachment_file_key",
                table: "attachment");

            migrationBuilder.DropIndex(
                name: "ix_attachment_owner_user_id",
                table: "attachment");

            migrationBuilder.DropColumn(
                name: "checksum_sha256",
                table: "attachment");

            migrationBuilder.DropColumn(
                name: "content_type",
                table: "attachment");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "attachment");

            migrationBuilder.DropColumn(
                name: "file_key",
                table: "attachment");

            migrationBuilder.DropColumn(
                name: "metadata_json",
                table: "attachment");

            migrationBuilder.DropColumn(
                name: "owner_user_id",
                table: "attachment");

            migrationBuilder.DropColumn(
                name: "status",
                table: "attachment");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "attachment");

            migrationBuilder.DropColumn(
                name: "version",
                table: "attachment");

            migrationBuilder.RenameColumn(
                name: "file_name",
                table: "attachment",
                newName: "filename");

            migrationBuilder.RenameColumn(
                name: "size_bytes",
                table: "attachment",
                newName: "uploader_id");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "attachment",
                newName: "uploaded_at");

            migrationBuilder.AlterColumn<long>(
                name: "card_id",
                table: "attachment",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "file_url",
                table: "attachment",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_attachment_uploader_id",
                table: "attachment",
                column: "uploader_id");

            migrationBuilder.AddForeignKey(
                name: "fk_attachment_card_card_id",
                table: "attachment",
                column: "card_id",
                principalTable: "card",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_attachment_user_uploader_id",
                table: "attachment",
                column: "uploader_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
