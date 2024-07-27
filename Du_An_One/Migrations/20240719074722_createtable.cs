using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Du_An_One.Migrations
{
    public partial class createtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KHACHHANG",
                columns: table => new
                {
                    MaKH = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NoiSinh = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CCCD = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    TenTaiKhoan = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    MatKhau = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TinhTrang = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHACHHANG", x => x.MaKH);
                });

            migrationBuilder.CreateTable(
                name: "KHUYENMAI",
                columns: table => new
                {
                    MaKhuyenMai = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    PhanTramKhuyenMai = table.Column<int>(type: "int", nullable: false),
                    ThoiGianStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHUYENMAI", x => x.MaKhuyenMai);
                });

            migrationBuilder.CreateTable(
                name: "NHACUNGCAP",
                columns: table => new
                {
                    MaNhaCC = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    TenNhaCC = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    NgayThanhLap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NguoiDaiDien = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ThoiGianCungCap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TinhTrang = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NHACUNGCAP", x => x.MaNhaCC);
                });

            migrationBuilder.CreateTable(
                name: "NHANVIEN",
                columns: table => new
                {
                    MaNV = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NoiSinh = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CCCD = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    NgayVaoLam = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Luong = table.Column<int>(type: "int", nullable: false),
                    VaiTro = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    TenTaiKhoan = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    MatKhau = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TinhTrang = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NHANVIEN", x => x.MaNV);
                });

            migrationBuilder.CreateTable(
                name: "HOADON",
                columns: table => new
                {
                    MaHoaDon = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    DiaChiNhanHang = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HTTT = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    TinhTrang = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    MaNV = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    NHANVIENMaNV = table.Column<string>(type: "nvarchar(5)", nullable: true),
                    MaKH = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    KHACHHANGMaKH = table.Column<string>(type: "nvarchar(5)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HOADON", x => x.MaHoaDon);
                    table.ForeignKey(
                        name: "FK_HOADON_KHACHHANG_KHACHHANGMaKH",
                        column: x => x.KHACHHANGMaKH,
                        principalTable: "KHACHHANG",
                        principalColumn: "MaKH");
                    table.ForeignKey(
                        name: "FK_HOADON_NHANVIEN_NHANVIENMaNV",
                        column: x => x.NHANVIENMaNV,
                        principalTable: "NHANVIEN",
                        principalColumn: "MaNV");
                });

            migrationBuilder.CreateTable(
                name: "SANPHAM",
                columns: table => new
                {
                    MaSP = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    TenSP = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    SoLuongBan = table.Column<int>(type: "int", nullable: false),
                    DonGiaBan = table.Column<double>(type: "float", nullable: false),
                    NgayNhap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DanhMucHang = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    KichCo = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HinhAnh = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    MaKhuyenMai = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    KHUYENMAIMaKhuyenMai = table.Column<string>(type: "nvarchar(5)", nullable: true),
                    MaNV = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    NHANVIENMaNV = table.Column<string>(type: "nvarchar(5)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SANPHAM", x => x.MaSP);
                    table.ForeignKey(
                        name: "FK_SANPHAM_KHUYENMAI_KHUYENMAIMaKhuyenMai",
                        column: x => x.KHUYENMAIMaKhuyenMai,
                        principalTable: "KHUYENMAI",
                        principalColumn: "MaKhuyenMai");
                    table.ForeignKey(
                        name: "FK_SANPHAM_NHANVIEN_NHANVIENMaNV",
                        column: x => x.NHANVIENMaNV,
                        principalTable: "NHANVIEN",
                        principalColumn: "MaNV");
                });

            migrationBuilder.CreateTable(
                name: "CHITIETHOADON",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHoaDon = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    MaSP = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    SoLuongMua = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<double>(type: "float", nullable: false),
                    HOADONMaHoaDon = table.Column<string>(type: "nvarchar(5)", nullable: true),
                    SANPHAMMaSP = table.Column<string>(type: "nvarchar(5)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHITIETHOADON", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CHITIETHOADON_HOADON_HOADONMaHoaDon",
                        column: x => x.HOADONMaHoaDon,
                        principalTable: "HOADON",
                        principalColumn: "MaHoaDon");
                    table.ForeignKey(
                        name: "FK_CHITIETHOADON_SANPHAM_SANPHAMMaSP",
                        column: x => x.SANPHAMMaSP,
                        principalTable: "SANPHAM",
                        principalColumn: "MaSP");
                });

            migrationBuilder.CreateTable(
                name: "CHITIETNHAP",
                columns: table => new
                {
                    MaChiTietNhap = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    MaNhaCC = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    NHACUNGCAPMaNhaCC = table.Column<string>(type: "nvarchar(5)", nullable: true),
                    MaSP = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    SANPHAMMaSP = table.Column<string>(type: "nvarchar(5)", nullable: true),
                    SoLuongNhap = table.Column<int>(type: "int", nullable: false),
                    DonGiaNhap = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHITIETNHAP", x => x.MaChiTietNhap);
                    table.ForeignKey(
                        name: "FK_CHITIETNHAP_NHACUNGCAP_NHACUNGCAPMaNhaCC",
                        column: x => x.NHACUNGCAPMaNhaCC,
                        principalTable: "NHACUNGCAP",
                        principalColumn: "MaNhaCC");
                    table.ForeignKey(
                        name: "FK_CHITIETNHAP_SANPHAM_SANPHAMMaSP",
                        column: x => x.SANPHAMMaSP,
                        principalTable: "SANPHAM",
                        principalColumn: "MaSP");
                });

            migrationBuilder.CreateTable(
                name: "HINHANH",
                columns: table => new
                {
                    MaHinhAnh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HinhAnh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaSP = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    SANPHAMMaSP = table.Column<string>(type: "nvarchar(5)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HINHANH", x => x.MaHinhAnh);
                    table.ForeignKey(
                        name: "FK_HINHANH_SANPHAM_SANPHAMMaSP",
                        column: x => x.SANPHAMMaSP,
                        principalTable: "SANPHAM",
                        principalColumn: "MaSP");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CHITIETHOADON_HOADONMaHoaDon",
                table: "CHITIETHOADON",
                column: "HOADONMaHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_CHITIETHOADON_SANPHAMMaSP",
                table: "CHITIETHOADON",
                column: "SANPHAMMaSP");

            migrationBuilder.CreateIndex(
                name: "IX_CHITIETNHAP_NHACUNGCAPMaNhaCC",
                table: "CHITIETNHAP",
                column: "NHACUNGCAPMaNhaCC");

            migrationBuilder.CreateIndex(
                name: "IX_CHITIETNHAP_SANPHAMMaSP",
                table: "CHITIETNHAP",
                column: "SANPHAMMaSP");

            migrationBuilder.CreateIndex(
                name: "IX_HINHANH_SANPHAMMaSP",
                table: "HINHANH",
                column: "SANPHAMMaSP");

            migrationBuilder.CreateIndex(
                name: "IX_HOADON_KHACHHANGMaKH",
                table: "HOADON",
                column: "KHACHHANGMaKH");

            migrationBuilder.CreateIndex(
                name: "IX_HOADON_NHANVIENMaNV",
                table: "HOADON",
                column: "NHANVIENMaNV");

            migrationBuilder.CreateIndex(
                name: "IX_SANPHAM_KHUYENMAIMaKhuyenMai",
                table: "SANPHAM",
                column: "KHUYENMAIMaKhuyenMai");

            migrationBuilder.CreateIndex(
                name: "IX_SANPHAM_NHANVIENMaNV",
                table: "SANPHAM",
                column: "NHANVIENMaNV");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CHITIETHOADON");

            migrationBuilder.DropTable(
                name: "CHITIETNHAP");

            migrationBuilder.DropTable(
                name: "HINHANH");

            migrationBuilder.DropTable(
                name: "HOADON");

            migrationBuilder.DropTable(
                name: "NHACUNGCAP");

            migrationBuilder.DropTable(
                name: "SANPHAM");

            migrationBuilder.DropTable(
                name: "KHACHHANG");

            migrationBuilder.DropTable(
                name: "KHUYENMAI");

            migrationBuilder.DropTable(
                name: "NHANVIEN");
        }
    }
}
