using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JTX_Ecom.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CustomerEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CustomerPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TransactionId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                });

            migrationBuilder.CreateTable(
                name: "Venues",
                columns: table => new
                {
                    VenueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venues", x => x.VenueId);
                });

            migrationBuilder.CreateTable(
                name: "Concerts",
                columns: table => new
                {
                    ConcertId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Artist = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Genre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    AvailableTickets = table.Column<int>(type: "int", nullable: false),
                    TotalTickets = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VenueId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Concerts", x => x.ConcertId);
                    table.ForeignKey(
                        name: "FK_Concerts_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "VenueId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    TicketId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TicketType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SeatNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Section = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoldAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConcertId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.TicketId);
                    table.ForeignKey(
                        name: "FK_Tickets_Concerts_ConcertId",
                        column: x => x.ConcertId,
                        principalTable: "Concerts",
                        principalColumn: "ConcertId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "Venues",
                columns: new[] { "VenueId", "Address", "Capacity", "City", "Country", "CreatedAt", "ImageUrl", "IsActive", "Name", "PhoneNumber", "State", "ZipCode" },
                values: new object[,]
                {
                    { 1, "4 Pennsylvania Plaza", 20000, "New York", "USA", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Madison Square Garden", "(212) 465-6741", "NY", "10001" },
                    { 2, "1111 S Figueroa St", 19000, "Los Angeles", "USA", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Staples Center", "(213) 742-7100", "CA", "90015" },
                    { 3, "131 W 3rd St", 500, "Chicago", "USA", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Blue Note", "(312) 555-0123", "IL", "60601" },
                    { 4, "1 Brickell City Centre", 10000, "Miami", "USA", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Ultra Arena", "(305) 555-0199", "FL", "33131" },
                    { 5, "2804 Opryland Dr", 4400, "Nashville", "USA", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Grand Ole Opry", "(615) 871-6779", "TN", "37214" },
                    { 6, "620 Atlantic Ave", 19000, "Brooklyn", "USA", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Barclays Center", "(917) 618-6100", "NY", "11217" }
                });

            migrationBuilder.InsertData(
                table: "Concerts",
                columns: new[] { "ConcertId", "Artist", "AvailableTickets", "BasePrice", "CreatedAt", "Description", "EventDate", "EventTime", "Genre", "ImageUrl", "IsActive", "Title", "TotalTickets", "UpdatedAt", "VenueId" },
                values: new object[,]
                {
                    { 1, "Various Rock Artists", 15000, 89.00m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The biggest rock festival of the year featuring top rock bands", new DateTime(2025, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 15, 19, 0, 0, 0, DateTimeKind.Unspecified), "Rock", "/assets/images/concerts/concert1.jpg", true, "Rock Festival 2025", 20000, null, 1 },
                    { 2, "Top Pop Stars", 18000, 125.00m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A spectacular pop music experience with chart-topping artists", new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 5, 20, 0, 0, 0, DateTimeKind.Unspecified), "Pop", "/assets/images/concerts/concert2.jpg", true, "Pop Extravaganza", 19000, null, 2 },
                    { 3, "Jazz Ensemble", 450, 65.00m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "An intimate evening of smooth jazz and soul", new DateTime(2025, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 20, 21, 0, 0, 0, DateTimeKind.Unspecified), "Jazz", "/assets/images/concerts/concert3.jpg", true, "Jazz Night Live", 500, null, 3 },
                    { 4, "Top EDM DJs", 9500, 149.00m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ultimate EDM experience with world-class DJs", new DateTime(2025, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 10, 22, 0, 0, 0, DateTimeKind.Unspecified), "Electronic", "/assets/images/concerts/concert4.jpg", true, "Electronic Dance Night", 10000, null, 4 },
                    { 5, "Country Legends", 4200, 79.00m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Celebrating country music heritage with legendary performers", new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 2, 19, 30, 0, 0, DateTimeKind.Unspecified), "Country", "/assets/images/concerts/concert5.jpg", true, "Country Music Fest", 4400, null, 5 },
                    { 6, "Hip Hop All-Stars", 17500, 95.00m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The hottest hip hop artists live on stage", new DateTime(2025, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 21, 20, 0, 0, 0, DateTimeKind.Unspecified), "Hip Hop", "/assets/images/concerts/concert6.jpg", true, "Hip Hop Summit", 19000, null, 6 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Concerts_EventDate",
                table: "Concerts",
                column: "EventDate");

            migrationBuilder.CreateIndex(
                name: "IX_Concerts_Genre",
                table: "Concerts",
                column: "Genre");

            migrationBuilder.CreateIndex(
                name: "IX_Concerts_VenueId",
                table: "Concerts",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerEmail",
                table: "Orders",
                column: "CustomerEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderDate",
                table: "Orders",
                column: "OrderDate");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNumber",
                table: "Orders",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ConcertId",
                table: "Tickets",
                column: "ConcertId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_OrderId",
                table: "Tickets",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Status",
                table: "Tickets",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TicketNumber",
                table: "Tickets",
                column: "TicketNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Venues_City",
                table: "Venues",
                column: "City");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Concerts");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Venues");
        }
    }
}
