using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WalletMate.DAL.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class DescriptionAndCommentFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Transaction",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Transaction",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Account_AccountNumber",
                table: "Account",
                column: "AccountNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Account_AccountNumber",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Transaction");
        }
    }
}
