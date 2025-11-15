using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JTX_Ecom.Migrations
{
    /// <inheritdoc />
    public partial class AddCartProfileAndMalaysianFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Section",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "SoldAt",
                table: "Tickets",
                newName: "UsedDate");

            migrationBuilder.AlterColumn<string>(
                name: "SeatNumber",
                table: "Tickets",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseDate",
                table: "Tickets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QRCode",
                table: "Tickets",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "Orders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress",
                table: "Orders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Orders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Orders",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Orders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.CartId);
                    table.ForeignKey(
                        name: "FK_Carts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    ProfileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ICNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ProfilePicture = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReceiveNewsletter = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.ProfileId);
                    table.ForeignKey(
                        name: "FK_UserProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    CartItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartId = table.Column<int>(type: "int", nullable: false),
                    ConcertId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TicketType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PricePerTicket = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.CartItemId);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "CartId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Concerts_ConcertId",
                        column: x => x.ConcertId,
                        principalTable: "Concerts",
                        principalColumn: "ConcertId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "ConcertId",
                keyValue: 1,
                columns: new[] { "Artist", "AvailableTickets", "BasePrice", "Description", "Title", "TotalTickets" },
                values: new object[] { "Various Artists", 80000, 250.00m, "The biggest music festival in Malaysia featuring top local and international artists", "Malaysian Music Festival 2025", 87000 });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "ConcertId",
                keyValue: 2,
                columns: new[] { "AvailableTickets", "BasePrice", "Description", "Title", "TotalTickets" },
                values: new object[] { 14500, 350.00m, "International pop stars live in Kuala Lumpur", "Pop Extravaganza KL", 16000 });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "ConcertId",
                keyValue: 3,
                columns: new[] { "Artist", "AvailableTickets", "BasePrice", "Description", "Title", "TotalTickets" },
                values: new object[] { "Jazz Ensemble Malaysia", 3200, 180.00m, "An intimate evening of smooth jazz featuring Malaysian and international artists", "Jazz Night KL", 3500 });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "ConcertId",
                keyValue: 4,
                columns: new[] { "Artist", "AvailableTickets", "BasePrice", "Description", "EventDate", "EventTime", "Genre", "Title", "TotalTickets", "VenueId" },
                values: new object[] { "World Music Artists", 4500, 220.00m, "Experience world music in the heart of Borneo", new DateTime(2025, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 14, 18, 0, 0, 0, DateTimeKind.Unspecified), "World Music", "Rainforest World Music Festival", 5000, 5 });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "ConcertId",
                keyValue: 5,
                columns: new[] { "Artist", "AvailableTickets", "BasePrice", "Description", "EventDate", "EventTime", "Genre", "Title", "TotalTickets", "VenueId" },
                values: new object[] { "Indie Bands Malaysia", 35000, 199.00m, "Malaysia's premier indie music festival", new DateTime(2025, 7, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 19, 14, 0, 0, 0, DateTimeKind.Unspecified), "Alternative", "Good Vibes Festival 2025", 40000, 6 });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "ConcertId",
                keyValue: 6,
                columns: new[] { "Artist", "AvailableTickets", "BasePrice", "Description", "EventDate", "EventTime", "Genre", "Title", "TotalTickets", "VenueId" },
                values: new object[] { "Top EDM DJs", 120000, 420.00m, "The ultimate EDM experience with world-class DJs", new DateTime(2025, 8, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 8, 9, 22, 0, 0, 0, DateTimeKind.Unspecified), "Electronic", "Electronic Dance Music Festival", 130000, 4 });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "VenueId",
                keyValue: 1,
                columns: new[] { "Address", "Capacity", "City", "Country", "Name", "PhoneNumber", "State", "ZipCode" },
                values: new object[] { "Bukit Jalil", 87411, "Kuala Lumpur", "Malaysia", "Bukit Jalil National Stadium", "03-8992 9000", "Wilayah Persekutuan", "57000" });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "VenueId",
                keyValue: 2,
                columns: new[] { "Address", "Capacity", "City", "Country", "Name", "PhoneNumber", "State", "ZipCode" },
                values: new object[] { "Jalan Barat, Bukit Jalil", 16000, "Kuala Lumpur", "Malaysia", "Axiata Arena", "03-8992 8833", "Wilayah Persekutuan", "57000" });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "VenueId",
                keyValue: 3,
                columns: new[] { "Address", "Capacity", "City", "Country", "Name", "PhoneNumber", "State", "ZipCode" },
                values: new object[] { "Jalan Lagenda", 3500, "Kuala Lumpur", "Malaysia", "Mega Star Arena", "03-7784 8000", "Wilayah Persekutuan", "59200" });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "VenueId",
                keyValue: 4,
                columns: new[] { "Address", "Capacity", "City", "Country", "Name", "PhoneNumber", "State", "ZipCode" },
                values: new object[] { "Jalan Pekeliling", 130000, "Sepang", "Malaysia", "Sepang International Circuit", "03-8778 2222", "Selangor", "64000" });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "VenueId",
                keyValue: 5,
                columns: new[] { "Address", "Capacity", "City", "Country", "Name", "PhoneNumber", "State", "ZipCode" },
                values: new object[] { "Jalan Tunku Abdul Rahman", 5000, "Kuching", "Malaysia", "Borneo Convention Centre Kuching", "082-423600", "Sarawak", "93100" });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "VenueId",
                keyValue: 6,
                columns: new[] { "Address", "Capacity", "City", "Country", "Name", "PhoneNumber", "State", "ZipCode" },
                values: new object[] { "Jalan Pemancar", 40000, "Bayan Lepas", "Malaysia", "Penang International Sports Arena", "04-643 6688", "Pulau Pinang", "11900" });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ConcertId",
                table: "CartItems",
                column: "ConcertId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserId",
                table: "UserProfiles",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropColumn(
                name: "PurchaseDate",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "QRCode",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "BillingAddress",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "UsedDate",
                table: "Tickets",
                newName: "SoldAt");

            migrationBuilder.AlterColumn<string>(
                name: "SeatNumber",
                table: "Tickets",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Section",
                table: "Tickets",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "Orders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "ConcertId",
                keyValue: 1,
                columns: new[] { "Artist", "AvailableTickets", "BasePrice", "Description", "Title", "TotalTickets" },
                values: new object[] { "Various Rock Artists", 15000, 89.00m, "The biggest rock festival of the year featuring top rock bands", "Rock Festival 2025", 20000 });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "ConcertId",
                keyValue: 2,
                columns: new[] { "AvailableTickets", "BasePrice", "Description", "Title", "TotalTickets" },
                values: new object[] { 18000, 125.00m, "A spectacular pop music experience with chart-topping artists", "Pop Extravaganza", 19000 });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "ConcertId",
                keyValue: 3,
                columns: new[] { "Artist", "AvailableTickets", "BasePrice", "Description", "Title", "TotalTickets" },
                values: new object[] { "Jazz Ensemble", 450, 65.00m, "An intimate evening of smooth jazz and soul", "Jazz Night Live", 500 });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "ConcertId",
                keyValue: 4,
                columns: new[] { "Artist", "AvailableTickets", "BasePrice", "Description", "EventDate", "EventTime", "Genre", "Title", "TotalTickets", "VenueId" },
                values: new object[] { "Top EDM DJs", 9500, 149.00m, "Ultimate EDM experience with world-class DJs", new DateTime(2025, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 10, 22, 0, 0, 0, DateTimeKind.Unspecified), "Electronic", "Electronic Dance Night", 10000, 4 });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "ConcertId",
                keyValue: 5,
                columns: new[] { "Artist", "AvailableTickets", "BasePrice", "Description", "EventDate", "EventTime", "Genre", "Title", "TotalTickets", "VenueId" },
                values: new object[] { "Country Legends", 4200, 79.00m, "Celebrating country music heritage with legendary performers", new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 2, 19, 30, 0, 0, DateTimeKind.Unspecified), "Country", "Country Music Fest", 4400, 5 });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "ConcertId",
                keyValue: 6,
                columns: new[] { "Artist", "AvailableTickets", "BasePrice", "Description", "EventDate", "EventTime", "Genre", "Title", "TotalTickets", "VenueId" },
                values: new object[] { "Hip Hop All-Stars", 17500, 95.00m, "The hottest hip hop artists live on stage", new DateTime(2025, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 21, 20, 0, 0, 0, DateTimeKind.Unspecified), "Hip Hop", "Hip Hop Summit", 19000, 6 });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "VenueId",
                keyValue: 1,
                columns: new[] { "Address", "Capacity", "City", "Country", "Name", "PhoneNumber", "State", "ZipCode" },
                values: new object[] { "4 Pennsylvania Plaza", 20000, "New York", "USA", "Madison Square Garden", "(212) 465-6741", "NY", "10001" });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "VenueId",
                keyValue: 2,
                columns: new[] { "Address", "Capacity", "City", "Country", "Name", "PhoneNumber", "State", "ZipCode" },
                values: new object[] { "1111 S Figueroa St", 19000, "Los Angeles", "USA", "Staples Center", "(213) 742-7100", "CA", "90015" });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "VenueId",
                keyValue: 3,
                columns: new[] { "Address", "Capacity", "City", "Country", "Name", "PhoneNumber", "State", "ZipCode" },
                values: new object[] { "131 W 3rd St", 500, "Chicago", "USA", "Blue Note", "(312) 555-0123", "IL", "60601" });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "VenueId",
                keyValue: 4,
                columns: new[] { "Address", "Capacity", "City", "Country", "Name", "PhoneNumber", "State", "ZipCode" },
                values: new object[] { "1 Brickell City Centre", 10000, "Miami", "USA", "Ultra Arena", "(305) 555-0199", "FL", "33131" });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "VenueId",
                keyValue: 5,
                columns: new[] { "Address", "Capacity", "City", "Country", "Name", "PhoneNumber", "State", "ZipCode" },
                values: new object[] { "2804 Opryland Dr", 4400, "Nashville", "USA", "Grand Ole Opry", "(615) 871-6779", "TN", "37214" });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "VenueId",
                keyValue: 6,
                columns: new[] { "Address", "Capacity", "City", "Country", "Name", "PhoneNumber", "State", "ZipCode" },
                values: new object[] { "620 Atlantic Ave", 19000, "Brooklyn", "USA", "Barclays Center", "(917) 618-6100", "NY", "11217" });
        }
    }
}
