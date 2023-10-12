using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetCafe.Server.Migrations
{
    /// <inheritdoc />
    public partial class RemovedCommentReplies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_ParentCommentId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ParentCommentId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ParentCommentId",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "24F1714A-340C-4EF5-99CE-63725043315E",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3b75d379-1ff8-4d0d-9302-55c8c3aac002", "AQAAAAIAAYagAAAAEAq070Fyv2eIUHWVCfe3NjIHcWA9BzPKlGp/zjS0VO3+FumkpVJnwRjC0vqyOqjq7A==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Comments");

            migrationBuilder.AddColumn<Guid>(
                name: "ParentCommentId",
                table: "Comments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "24F1714A-340C-4EF5-99CE-63725043315E",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "75bff0df-3625-4114-9147-4685019f4ace", "AQAAAAIAAYagAAAAENuC1/6Er2cy0DYATSoonS9M2oVnncnvGUKb9kfeBINjszWaaZzNl16UJJmShoT2Hw==" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentCommentId",
                table: "Comments",
                column: "ParentCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_ParentCommentId",
                table: "Comments",
                column: "ParentCommentId",
                principalTable: "Comments",
                principalColumn: "Id");
        }
    }
}
