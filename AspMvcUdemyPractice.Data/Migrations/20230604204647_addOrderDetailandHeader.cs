using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspMvcUdemyPractice.Data.Migrations
{
    /// <inheritdoc />
    public partial class addOrderDetailandHeader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MyProperty",
                table: "OrderDetails",
                newName: "Price");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "OrderDetails",
                newName: "MyProperty");
        }
    }
}
