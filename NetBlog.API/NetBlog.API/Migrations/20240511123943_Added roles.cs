using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NetBlog.API.Migrations
{
    /// <inheritdoc />
    public partial class Addedroles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AuthorName",
                table: "Comments",
                newName: "AuthorId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "772101b0-46d6-4422-b672-5e1aca8db961", "772101b0-46d6-4422-b672-5e1aca8db961", "Reader", "READER" },
                    { "a249e775-cdf9-472c-bc53-955e306f0f98", "a249e775-cdf9-472c-bc53-955e306f0f98", "Author", "AUTHOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "772101b0-46d6-4422-b672-5e1aca8db961");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a249e775-cdf9-472c-bc53-955e306f0f98");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Comments",
                newName: "AuthorName");
        }
    }
}
