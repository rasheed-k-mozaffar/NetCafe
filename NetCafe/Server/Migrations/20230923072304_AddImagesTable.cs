using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetCafe.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddImagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverImage",
                table: "Posts");

            migrationBuilder.AddColumn<Guid>(
                name: "CoverImageId",
                table: "Series",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Series",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverImageUrl",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SeriesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "24F1714A-340C-4EF5-99CE-63725043315E",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1927e25c-0af0-4624-8fdc-02cd7c4dd583", "AQAAAAIAAYagAAAAELi9RDAgrqT7tSFOh6j0ufE+WtmUCRcaPaO9J0U0NpWYm8AODswf5oQxjgAtsqReOA==" });

            migrationBuilder.CreateIndex(
                name: "IX_Series_CoverImageId",
                table: "Series",
                column: "CoverImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_PostId",
                table: "Images",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Series_Images_CoverImageId",
                table: "Series",
                column: "CoverImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Series_Images_CoverImageId",
                table: "Series");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Series_CoverImageId",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "CoverImageId",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "CoverImageUrl",
                table: "Posts");

            migrationBuilder.AddColumn<byte[]>(
                name: "CoverImage",
                table: "Posts",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "24F1714A-340C-4EF5-99CE-63725043315E",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f03e5f80-d2fe-4b3b-b960-ee2214410a2b", "AQAAAAIAAYagAAAAENdi1hNXVeGFyW38AllDyW8Z7BROqG6GAo/yk/YzXONUczKanlf9qUhnZdRHpXAZ0w==" });
        }
    }
}
