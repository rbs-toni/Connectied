using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Connectied.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GuestLists",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Group = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Event1Quota = table.Column<int>(type: "int", nullable: false),
                    Event2Quota = table.Column<int>(type: "int", nullable: false),
                    Event1Rsvp = table.Column<int>(type: "int", nullable: false),
                    Event2Rsvp = table.Column<int>(type: "int", nullable: false),
                    Event1Attend = table.Column<int>(type: "int", nullable: false),
                    Event2Attend = table.Column<int>(type: "int", nullable: false),
                    Event2AngpaoCount = table.Column<int>(type: "int", nullable: false),
                    Event2GiftCount = table.Column<int>(type: "int", nullable: false),
                    Event2Souvenir = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuestLists", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuestLists");
        }
    }
}
