using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FakeShippingCompanyApi.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shipments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderCompany = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SenderCompanyAdddress = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    SenderEmail = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    SenderPhone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    DeliveryTargetAddress = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    DeliveryTargetFullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DeliveryTargetEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DeliveryPhone = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    TrackingNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipments", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shipments");
        }
    }
}
