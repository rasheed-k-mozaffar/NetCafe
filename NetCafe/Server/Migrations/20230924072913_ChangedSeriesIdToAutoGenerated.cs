﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetCafe.Server.Migrations
{
    /// <inheritdoc />
    public partial class ChangedSeriesIdToAutoGenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "24F1714A-340C-4EF5-99CE-63725043315E",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b649553b-1dc6-4808-81f2-9b5bb16118c3", "AQAAAAIAAYagAAAAEIC6UXs8fG/X2MBRpKD6YsKSZi0h3q0uF3RxmMtOh2g+CSOueOge5gf8L/KaGoEccA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "24F1714A-340C-4EF5-99CE-63725043315E",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6bcc7762-80de-4c6f-bb8b-78252217e740", "AQAAAAIAAYagAAAAEPlT8UXdNho8tEjPJZ7y6Ep0MpIq7CxkCxEYv9D7ibTQZVhRk93riLfoEUC6DCYXwQ==" });
        }
    }
}
