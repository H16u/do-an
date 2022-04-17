namespace BTL_N9.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DonHang")]
    public partial class DonHang
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string MaDonHang { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string MaSanPham { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string MaKhachHang { get; set; }

        public int SoLuong { get; set; }

        [Column(TypeName = "date")]
        public DateTime ThoiGian { get; set; }

        public bool TinhTrang { get; set; }

        public virtual KhachHang KhachHang { get; set; }

        public virtual SanPham SanPham { get; set; }
    }
}
