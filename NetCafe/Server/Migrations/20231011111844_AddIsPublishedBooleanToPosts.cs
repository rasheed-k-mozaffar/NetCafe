using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetCafe.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPublishedBooleanToPosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Posts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "24F1714A-340C-4EF5-99CE-63725043315E",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "75bff0df-3625-4114-9147-4685019f4ace", "AQAAAAIAAYagAAAAENuC1/6Er2cy0DYATSoonS9M2oVnncnvGUKb9kfeBINjszWaaZzNl16UJJmShoT2Hw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Posts");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "24F1714A-340C-4EF5-99CE-63725043315E",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b649553b-1dc6-4808-81f2-9b5bb16118c3", "AQAAAAIAAYagAAAAEIC6UXs8fG/X2MBRpKD6YsKSZi0h3q0uF3RxmMtOh2g+CSOueOge5gf8L/KaGoEccA==" });
        }
    }
}
