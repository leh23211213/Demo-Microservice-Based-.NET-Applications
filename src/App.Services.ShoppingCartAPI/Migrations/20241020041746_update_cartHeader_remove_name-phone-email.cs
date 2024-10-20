using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Services.ShoppingCartAPI.Migrations
{
    /// <inheritdoc />
    public partial class update_cartHeader_remove_namephoneemail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "CartHeaders");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CartHeaders");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "CartHeaders");

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "CartDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "CartHeaders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CartHeaders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "CartHeaders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "CartDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
