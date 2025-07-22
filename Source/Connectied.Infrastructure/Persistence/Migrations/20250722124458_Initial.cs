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
            migrationBuilder.EnsureSchema(
                name: "Connectied");

            migrationBuilder.CreateTable(
                name: "GuestGroups",
                schema: "Connectied",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuestGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuestListConfigurations",
                schema: "Connectied",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Columns = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Groups = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IncludedGuests = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExcludedGuests = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuestListConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Guests",
                schema: "Connectied",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupId = table.Column<string>(type: "nvarchar(36)", nullable: true),
                    Event1Quota = table.Column<int>(type: "int", nullable: false),
                    Event2Quota = table.Column<int>(type: "int", nullable: false),
                    Event1RSVP = table.Column<int>(type: "int", nullable: false),
                    Event2RSVP = table.Column<int>(type: "int", nullable: false),
                    Event1Attendance = table.Column<int>(type: "int", nullable: false),
                    Event2Attendance = table.Column<int>(type: "int", nullable: false),
                    Event1Angpao = table.Column<int>(type: "int", nullable: false),
                    Event2Angpao = table.Column<int>(type: "int", nullable: false),
                    Event1Gift = table.Column<int>(type: "int", nullable: false),
                    Event2Gift = table.Column<int>(type: "int", nullable: false),
                    Event1Souvenir = table.Column<int>(type: "int", nullable: false),
                    Event2Souvenir = table.Column<int>(type: "int", nullable: false),
                    Event1RSVPStatus = table.Column<int>(type: "int", nullable: false),
                    Event2RSVPStatus = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<string>(type: "nvarchar(36)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Event1CheckedIn = table.Column<bool>(type: "bit", nullable: false),
                    Event2CheckedIn = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Guests_GuestGroups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "Connectied",
                        principalTable: "GuestGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Guests_Guests_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Connectied",
                        principalTable: "Guests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GuestLists",
                schema: "Connectied",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LinkCode = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    ConfigurationId = table.Column<string>(type: "nvarchar(36)", nullable: true),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuestLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuestLists_GuestListConfigurations_ConfigurationId",
                        column: x => x.ConfigurationId,
                        principalSchema: "Connectied",
                        principalTable: "GuestListConfigurations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GuestRegistries",
                schema: "Connectied",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    GuestId = table.Column<string>(type: "nvarchar(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuestRegistries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuestRegistries_Guests_GuestId",
                        column: x => x.GuestId,
                        principalSchema: "Connectied",
                        principalTable: "Guests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuestLists_ConfigurationId",
                schema: "Connectied",
                table: "GuestLists",
                column: "ConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_GuestRegistries_GuestId",
                schema: "Connectied",
                table: "GuestRegistries",
                column: "GuestId");

            migrationBuilder.CreateIndex(
                name: "IX_Guests_GroupId",
                schema: "Connectied",
                table: "Guests",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Guests_ParentId",
                schema: "Connectied",
                table: "Guests",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuestLists",
                schema: "Connectied");

            migrationBuilder.DropTable(
                name: "GuestRegistries",
                schema: "Connectied");

            migrationBuilder.DropTable(
                name: "GuestListConfigurations",
                schema: "Connectied");

            migrationBuilder.DropTable(
                name: "Guests",
                schema: "Connectied");

            migrationBuilder.DropTable(
                name: "GuestGroups",
                schema: "Connectied");
        }
    }
}
