using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KanaMelody.Migrations
{
    /// <inheritdoc />
    public partial class DiscNumberMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscNumber",
                table: "SongMetadatas",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscNumber",
                table: "SongMetadatas");
        }
    }
}
