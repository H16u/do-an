namespace BTL_N9.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SanPham")]
    public partial class SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPham()
        {
            DonHang = new HashSet<DonHang>();
            GioHang = new HashSet<GioHang>();
        }

        [Key]
        [StringLength(20)]
        public string MaSanPham { get; set; }

        [Required]
        [StringLength(50)]
        public string TenSanPham { get; set; }

        [Required]
        [StringLength(20)]
        public string MaThuongHieu { get; set; }

        [Required]
        [StringLength(100)]
        public string TomTat { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Anh { get; set; }

        public int SoLuongNhap { get; set; }

        public int SoLuongTonKho { get; set; }

        [Column(TypeName = "money")]
        public decimal GiaNhap { get; set; }

        [Column(TypeName = "money")]
        public decimal GiaDeXuat { get; set; }

        [Column(TypeName = "money")]
        public decimal GiaBan { get; set; }

        [Required]
        [StringLength(50)]
        public string BaoHiem { get; set; }

        [Required]
        [StringLength(50)]
        public string BaoHanh { get; set; }

        [Required]
        [StringLength(50)]
        public string ThamDinhThatGia { get; set; }

        [Required]
        [StringLength(50)]
        public string GiaoHang { get; set; }

        [Required]
        [StringLength(50)]
        public string ThuongHieu { get; set; }

        [Required]
        [StringLength(50)]
        public string XuatXu { get; set; }

        [Required]
        [StringLength(50)]
        public string DHDanhCho { get; set; }

        [Required]
        [StringLength(50)]
        public string KieuMay { get; set; }

        [Required]
        [StringLength(50)]
        public string KichCo { get; set; }

        [Required]
        [StringLength(50)]
        public string ChatLieuVo { get; set; }

        [Required]
        [StringLength(50)]
        public string ChatLieuDay { get; set; }

        [Required]
        [StringLength(50)]
        public string ChatLieuKinh { get; set; }

        [Required]
        [StringLength(50)]
        public string ChucNang { get; set; }

        [Required]
        [StringLength(50)]
        public string DoChiuNuoc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonHang> DonHang { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GioHang> GioHang { get; set; }

        public virtual ThuongHieu ThuongHieu1 { get; set; }
    }
}
