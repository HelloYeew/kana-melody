using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KanaMelody.Migrations
{
    /// <inheritdoc />
    public partial class AutoIncrementKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrackNumber",
                table: "SongMetadatas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrackNumber",
                table: "SongMetadatas");
        }
    }
}
