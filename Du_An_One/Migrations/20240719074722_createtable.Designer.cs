﻿// <auto-generated />
using System;
using Du_An_One.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Du_An_One.Migrations
{
    [DbContext(typeof(Du_An_OneContext))]
    [Migration("20240719074722_createtable")]
    partial class createtable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.32")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Du_An_One.Models.CHITIETHOADON", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<double>("DonGia")
                        .HasColumnType("float");

                    b.Property<string>("HOADONMaHoaDon")
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("MaHoaDon")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("MaSP")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("SANPHAMMaSP")
                        .HasColumnType("nvarchar(5)");

                    b.Property<int>("SoLuongMua")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("HOADONMaHoaDon");

                    b.HasIndex("SANPHAMMaSP");

                    b.ToTable("CHITIETHOADON");
                });

            modelBuilder.Entity("Du_An_One.Models.CHITIETNHAP", b =>
                {
                    b.Property<string>("MaChiTietNhap")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<double>("DonGiaNhap")
                        .HasColumnType("float");

                    b.Property<string>("MaNhaCC")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("MaSP")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("NHACUNGCAPMaNhaCC")
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("SANPHAMMaSP")
                        .HasColumnType("nvarchar(5)");

                    b.Property<int>("SoLuongNhap")
                        .HasColumnType("int");

                    b.HasKey("MaChiTietNhap");

                    b.HasIndex("NHACUNGCAPMaNhaCC");

                    b.HasIndex("SANPHAMMaSP");

                    b.ToTable("CHITIETNHAP");
                });

            modelBuilder.Entity("Du_An_One.Models.HINHANH", b =>
                {
                    b.Property<int>("MaHinhAnh")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaHinhAnh"), 1L, 1);

                    b.Property<string>("HinhAnh")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("MaSP")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("SANPHAMMaSP")
                        .HasColumnType("nvarchar(5)");

                    b.HasKey("MaHinhAnh");

                    b.HasIndex("SANPHAMMaSP");

                    b.ToTable("HINHANH");
                });

            modelBuilder.Entity("Du_An_One.Models.HOADON", b =>
                {
                    b.Property<string>("MaHoaDon")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("DiaChiNhanHang")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("HTTT")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("KHACHHANGMaKH")
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("MaKH")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("MaNV")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("NHANVIENMaNV")
                        .HasColumnType("nvarchar(5)");

                    b.Property<DateTime>("NgayTao")
                        .HasColumnType("datetime2");

                    b.Property<string>("TinhTrang")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("MaHoaDon");

                    b.HasIndex("KHACHHANGMaKH");

                    b.HasIndex("NHANVIENMaNV");

                    b.ToTable("HOADON");
                });

            modelBuilder.Entity("Du_An_One.Models.KHACHHANG", b =>
                {
                    b.Property<string>("MaKH")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("CCCD")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("DiaChi")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("HoTen")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("MatKhau")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime?>("NgaySinh")
                        .HasColumnType("datetime2");

                    b.Property<string>("NoiSinh")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("SDT")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("TenTaiKhoan")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("TinhTrang")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasKey("MaKH");

                    b.ToTable("KHACHHANG");
                });

            modelBuilder.Entity("Du_An_One.Models.KHUYENMAI", b =>
                {
                    b.Property<string>("MaKhuyenMai")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<int>("PhanTramKhuyenMai")
                        .HasColumnType("int");

                    b.Property<DateTime>("ThoiGianEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ThoiGianStart")
                        .HasColumnType("datetime2");

                    b.HasKey("MaKhuyenMai");

                    b.ToTable("KHUYENMAI");
                });

            modelBuilder.Entity("Du_An_One.Models.NHACUNGCAP", b =>
                {
                    b.Property<string>("MaNhaCC")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("DiaChi")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<DateTime>("NgayThanhLap")
                        .HasColumnType("datetime2");

                    b.Property<string>("NguoiDaiDien")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("SDT")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("TenNhaCC")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<DateTime>("ThoiGianCungCap")
                        .HasColumnType("datetime2");

                    b.Property<string>("TinhTrang")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasKey("MaNhaCC");

                    b.ToTable("NHACUNGCAP");
                });

            modelBuilder.Entity("Du_An_One.Models.NHANVIEN", b =>
                {
                    b.Property<string>("MaNV")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("CCCD")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("DiaChi")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("HoTen")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<int>("Luong")
                        .HasColumnType("int");

                    b.Property<string>("MatKhau")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime?>("NgaySinh")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("NgayVaoLam")
                        .HasColumnType("datetime2");

                    b.Property<string>("NoiSinh")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("SDT")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("TenTaiKhoan")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("TinhTrang")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("VaiTro")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasKey("MaNV");

                    b.ToTable("NHANVIEN");
                });

            modelBuilder.Entity("Du_An_One.Models.SANPHAM", b =>
                {
                    b.Property<string>("MaSP")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("DanhMucHang")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<double>("DonGiaBan")
                        .HasColumnType("float");

                    b.Property<string>("KHUYENMAIMaKhuyenMai")
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("KichCo")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<string>("MaKhuyenMai")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("MaNV")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("MoTa")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HinhAnh")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NHANVIENMaNV")
                        .HasColumnType("nvarchar(5)");

                    b.Property<DateTime>("NgayNhap")
                        .HasColumnType("datetime2");

                    b.Property<int>("SoLuongBan")
                        .HasColumnType("int");

                    b.Property<string>("TenSP")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("MaSP");

                    b.HasIndex("KHUYENMAIMaKhuyenMai");

                    b.HasIndex("NHANVIENMaNV");

                    b.ToTable("SANPHAM");
                });

            modelBuilder.Entity("Du_An_One.Models.CHITIETHOADON", b =>
                {
                    b.HasOne("Du_An_One.Models.HOADON", "HOADON")
                        .WithMany("CHITIETHOADONs")
                        .HasForeignKey("HOADONMaHoaDon");

                    b.HasOne("Du_An_One.Models.SANPHAM", "SANPHAM")
                        .WithMany("CHITIETHOADONs")
                        .HasForeignKey("SANPHAMMaSP");

                    b.Navigation("HOADON");

                    b.Navigation("SANPHAM");
                });

            modelBuilder.Entity("Du_An_One.Models.CHITIETNHAP", b =>
                {
                    b.HasOne("Du_An_One.Models.NHACUNGCAP", "NHACUNGCAP")
                        .WithMany("CHITIETNHAPs")
                        .HasForeignKey("NHACUNGCAPMaNhaCC");

                    b.HasOne("Du_An_One.Models.SANPHAM", "SANPHAM")
                        .WithMany("CHITIETNHAPs")
                        .HasForeignKey("SANPHAMMaSP");

                    b.Navigation("NHACUNGCAP");

                    b.Navigation("SANPHAM");
                });

            modelBuilder.Entity("Du_An_One.Models.HINHANH", b =>
                {
                    b.HasOne("Du_An_One.Models.SANPHAM", "SANPHAM")
                        .WithMany("HINHANHs")
                        .HasForeignKey("SANPHAMMaSP");

                    b.Navigation("SANPHAM");
                });

            modelBuilder.Entity("Du_An_One.Models.HOADON", b =>
                {
                    b.HasOne("Du_An_One.Models.KHACHHANG", "KHACHHANG")
                        .WithMany("HOADONs")
                        .HasForeignKey("KHACHHANGMaKH");

                    b.HasOne("Du_An_One.Models.NHANVIEN", "NHANVIEN")
                        .WithMany("HOADONs")
                        .HasForeignKey("NHANVIENMaNV");

                    b.Navigation("KHACHHANG");

                    b.Navigation("NHANVIEN");
                });

            modelBuilder.Entity("Du_An_One.Models.SANPHAM", b =>
                {
                    b.HasOne("Du_An_One.Models.KHUYENMAI", "KHUYENMAI")
                        .WithMany("SANPHAMs")
                        .HasForeignKey("KHUYENMAIMaKhuyenMai");

                    b.HasOne("Du_An_One.Models.NHANVIEN", "NHANVIEN")
                        .WithMany("SANPHAMs")
                        .HasForeignKey("NHANVIENMaNV");

                    b.Navigation("KHUYENMAI");

                    b.Navigation("NHANVIEN");
                });

            modelBuilder.Entity("Du_An_One.Models.HOADON", b =>
                {
                    b.Navigation("CHITIETHOADONs");
                });

            modelBuilder.Entity("Du_An_One.Models.KHACHHANG", b =>
                {
                    b.Navigation("HOADONs");
                });

            modelBuilder.Entity("Du_An_One.Models.KHUYENMAI", b =>
                {
                    b.Navigation("SANPHAMs");
                });

            modelBuilder.Entity("Du_An_One.Models.NHACUNGCAP", b =>
                {
                    b.Navigation("CHITIETNHAPs");
                });

            modelBuilder.Entity("Du_An_One.Models.NHANVIEN", b =>
                {
                    b.Navigation("HOADONs");

                    b.Navigation("SANPHAMs");
                });

            modelBuilder.Entity("Du_An_One.Models.SANPHAM", b =>
                {
                    b.Navigation("CHITIETHOADONs");

                    b.Navigation("CHITIETNHAPs");

                    b.Navigation("HINHANHs");
                });
#pragma warning restore 612, 618
        }
    }
}
