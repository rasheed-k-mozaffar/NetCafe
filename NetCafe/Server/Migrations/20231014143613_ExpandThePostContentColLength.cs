using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetCafe.Server.Migrations
{
    /// <inheritdoc />
    public partial class ExpandThePostContentColLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "24F1714A-340C-4EF5-99CE-63725043315E",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b18b0570-7529-4118-a206-39d64a3c1500", "AQAAAAIAAYagAAAAEHBDd74ti2IxU3G2d/RVacrktNrJC9ZNsT6i4lYyyCi9Z9yNsulLwpAKry8aCkjpOA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "24F1714A-340C-4EF5-99CE-63725043315E",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3b75d379-1ff8-4d0d-9302-55c8c3aac002", "AQAAAAIAAYagAAAAEAq070Fyv2eIUHWVCfe3NjIHcWA9BzPKlGp/zjS0VO3+FumkpVJnwRjC0vqyOqjq7A==" });
        }
    }
}
