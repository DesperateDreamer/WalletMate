using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WalletMate.DAL.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddBillingAddressToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BillingAddress",
                table: "User",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillingAddress",
                table: "User");
        }
    }
}
