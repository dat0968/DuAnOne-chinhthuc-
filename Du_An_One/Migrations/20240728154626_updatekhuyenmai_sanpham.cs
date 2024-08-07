using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Du_An_One.Migrations
{
    public partial class updatekhuyenmai_sanpham : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SANPHAM_KHUYENMAI_KHUYENMAIMaKhuyenMai",
                table: "SANPHAM");

            migrationBuilder.DropIndex(
                name: "IX_SANPHAM_KHUYENMAIMaKhuyenMai",
                table: "SANPHAM");

            migrationBuilder.DropColumn(
                name: "KHUYENMAIMaKhuyenMai",
                table: "SANPHAM");

            migrationBuilder.CreateIndex(
                name: "IX_SANPHAM_MaKhuyenMai",
                table: "SANPHAM",
                column: "MaKhuyenMai");

            migrationBuilder.AddForeignKey(
                name: "FK_SANPHAM_KHUYENMAI_MaKhuyenMai",
                table: "SANPHAM",
                column: "MaKhuyenMai",
                principalTable: "KHUYENMAI",
                principalColumn: "MaKhuyenMai",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SANPHAM_KHUYENMAI_MaKhuyenMai",
                table: "SANPHAM");

            migrationBuilder.DropIndex(
                name: "IX_SANPHAM_MaKhuyenMai",
                table: "SANPHAM");

            migrationBuilder.AddColumn<string>(
                name: "KHUYENMAIMaKhuyenMai",
                table: "SANPHAM",
                type: "nvarchar(5)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SANPHAM_KHUYENMAIMaKhuyenMai",
                table: "SANPHAM",
                column: "KHUYENMAIMaKhuyenMai");

            migrationBuilder.AddForeignKey(
                name: "FK_SANPHAM_KHUYENMAI_KHUYENMAIMaKhuyenMai",
                table: "SANPHAM",
                column: "KHUYENMAIMaKhuyenMai",
                principalTable: "KHUYENMAI",
                principalColumn: "MaKhuyenMai");
        }
    }
}
