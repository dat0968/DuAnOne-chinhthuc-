using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Du_An_One.Models;

namespace Du_An_One.Data
{
    public class Du_An_OneContext : DbContext
    {
        public Du_An_OneContext (DbContextOptions<Du_An_OneContext> options)
            : base(options)
        {
        }

        public DbSet<Du_An_One.Models.NHANVIEN> NHANVIEN { get; set; } = default!;

        public DbSet<Du_An_One.Models.KHACHHANG>? KHACHHANG { get; set; }

        public DbSet<Du_An_One.Models.KHUYENMAI>? KHUYENMAI { get; set; }

        public DbSet<Du_An_One.Models.SANPHAM>? SANPHAM { get; set; }

        public DbSet<Du_An_One.Models.HOADON>? HOADON { get; set; }

        public DbSet<Du_An_One.Models.CHITIETHOADON>? CHITIETHOADON { get; set; }

        public DbSet<Du_An_One.Models.NHACUNGCAP>? NHACUNGCAP { get; set; }

        public DbSet<Du_An_One.Models.CHITIETNHAP>? CHITIETNHAP { get; set; }

        public DbSet<Du_An_One.Models.HINHANH>? HINHANH { get; set; }
    }
}
