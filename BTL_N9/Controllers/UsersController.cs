using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BTL_N9.Models;
using System.Security.Cryptography;
using System.Text;
using PagedList;


namespace BTL_N9.Controllers
{
    public class UsersController : Controller
    {
        private BTLDB db = new BTLDB();
        private static string maKhachHang = "";

        //Lấy dữ liệu ra trang chủ
        public ActionResult TrangChu()
        {
            ViewBag.tongTien = GiaTriGioHang();
            ViewBag.maKHang = maKhachHang;

            var sanPham = db.SanPham.Include(s => s.ThuongHieu1);
            return View(sanPham.ToList());
        }

        //Danh sách tìm kiếm
        public ActionResult ViewDanhSach(string searchString, int? page, string sortOrder, string currentFilter)
        {
            ViewBag.tongTien = GiaTriGioHang();
            ViewBag.maKHang = maKhachHang;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.SapTheoTenAZ = String.IsNullOrEmpty(sortOrder) ? "ten_AZ" : "";
            ViewBag.SapTheoTenZA = sortOrder == "ten_ZA" ? "ten_ZA" : "ten_ZA";
            ViewBag.SapTheoGiaAZ = sortOrder == "gia_AZ" ? "gia_AZ" : "gia_AZ";
            ViewBag.SapTheoGiaZA = sortOrder == "gia_ZA" ? "gia_ZA" : "gia_ZA";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var sanPham = db.SanPham.Include(s => s.ThuongHieu1);
            if (!String.IsNullOrEmpty(searchString))
            {
                sanPham = sanPham.Where(s => s.TenSanPham.Contains(searchString) || s.DHDanhCho.Contains(searchString));
                ViewBag.TimKiem = searchString;
            }

            switch (sortOrder)
            {
                case "ten_AZ":
                    sanPham = sanPham.OrderBy(s => s.TenSanPham);
                    break;
                case "ten_ZA":
                    sanPham = sanPham.OrderByDescending(s => s.TenSanPham);
                    break;
                case "gia_AZ":
                    sanPham = sanPham.OrderBy(s => s.GiaBan);
                    break;
                case "gia_ZA":
                    sanPham = sanPham.OrderByDescending(s => s.GiaBan);
                    break;
                default:
                    sanPham = sanPham.OrderBy(s => s.TenSanPham);
                    break;
            }

            int pageSize = 16;
            int pageNumBer = (page ?? 1);
            return View(sanPham.ToPagedList(pageNumBer, pageSize));
        }

        //Thông tin về Shopwatch
        public ActionResult ViewVeShopwatch()
        {
            ViewBag.tongTien = GiaTriGioHang();
            ViewBag.maKHang = maKhachHang;

            return View();
        }

        //Liên hệ
        public ActionResult ViewLienHe()
        {
            ViewBag.tongTien = GiaTriGioHang();
            ViewBag.maKHang = maKhachHang;

            return View();
        }

        //GET:View chi tiết sản phẩm
        public ActionResult ViewSanPham(string id)
        {
            ViewBag.tongTien = GiaTriGioHang();
            ViewBag.maKHang = maKhachHang;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPham.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ViewSanPham()
        {
            ViewBag.tongTien = GiaTriGioHang();
            ViewBag.maKHang = maKhachHang;

            if (ModelState.IsValid)
            {
                var masp = Request.Form["MaSanPham"];
                var sl = Request.Form["soluongsanpham"];
                Session["MaKhachHang"] = masp + sl + "999";
                GioHang item = db.GioHang.SingleOrDefault(g => g.MaSanPham == masp);
                if (item == null)
                {
                    GioHang gh = new GioHang();
                    gh.MaSanPham = masp;
                    gh.SoLuong = int.Parse(sl);
                    db.GioHang.Add(gh);
                    db.SaveChanges();
                }
                else
                {

                    GioHang gh = new GioHang();
                    gh.MaSanPham = masp;
                    gh.SoLuong = item.SoLuong + int.Parse(sl);
                    db.GioHang.Remove(item);
                    db.GioHang.Add(gh);
                    db.SaveChanges();
                }
                return RedirectToAction("ViewGioHang");
            }
            return View();
        }

        //Get: giỏ hàng
        public ActionResult ViewGioHang()
        {
            ViewBag.tongTien = GiaTriGioHang();
            ViewBag.maKHang = maKhachHang;

            var gioHang = db.GioHang.Include(g => g.SanPham);
            gioHang = gioHang.OrderBy(g => g.MaSanPham);
            return View(gioHang.ToList());
        }

        //POST: giỏ hàng
        [HttpPost]
        public ActionResult ViewGioHang(string abc)
        {
            ViewBag.tongTien = GiaTriGioHang();
            ViewBag.maKHang = maKhachHang;

            if (ModelState.IsValid)
            {
                var giohang = db.GioHang.Include(g => g.SanPham);
                foreach (var item in giohang)
                {
                    var sl = Request.Form[item.MaSanPham];
                    if (int.Parse(sl) > 0)
                    {
                        GioHang gh = new GioHang();
                        gh.MaSanPham = item.MaSanPham;
                        gh.SoLuong = int.Parse(sl);
                        db.GioHang.Remove(item);
                        db.GioHang.Add(gh);
                    }
                    else
                    {
                        db.GioHang.Remove(item);
                    }
                }
                db.SaveChanges();
                giohang = giohang.OrderBy(g => g.MaSanPham);
                return View(giohang.ToList());
            }
            return View();
        }

        public ActionResult XoaSanPham(string id)
        {
            var sp = db.GioHang.SingleOrDefault(s => s.MaSanPham == id);
            db.GioHang.Remove(sp);
            db.SaveChanges();
            return RedirectToAction("ViewGioHang");
        }

        //GET: View thanh toán đơn hàng
        public ActionResult ViewThanhToan()
        {
            ViewBag.tongTien = GiaTriGioHang();
            ViewBag.maKHang = maKhachHang;


            ViewBag.hoTen = "";
            ViewBag.diaChi = "";
            ViewBag.soDienThoai = "";
            ViewBag.email = "";
            ViewBag.maKH = "";
            if (maKhachHang != "")
            {
                KhachHang kh = db.KhachHang.SingleOrDefault(k => k.MaKhachHang == maKhachHang);
                ViewBag.hoTen = kh.TenKhachHang;
                ViewBag.diaChi = kh.DiaChi;
                ViewBag.soDienThoai = kh.SoDienThoai;
                ViewBag.email = kh.Email;
                ViewBag.maKH = maKhachHang;
            }

            var gioHang = db.GioHang.Include(g => g.SanPham);
            gioHang = gioHang.OrderBy(g => g.MaSanPham);
            return View(gioHang.ToList());
        }

        [HttpPost]
        public ActionResult ViewThanhToan(string tenDangNhap, string matKhau)
        {
            ViewBag.tongTien = GiaTriGioHang();
            ViewBag.maKHang = maKhachHang;



            if (ModelState.IsValid)
            {
                ViewBag.hoTen = "";
                ViewBag.diaChi = "";
                ViewBag.soDienThoai = "";
                ViewBag.email = "";
                ViewBag.maKH = "";

                var pw = GetMD5(matKhau);
                var data = db.KhachHang.Where(s => s.TenDangNhap.Equals(tenDangNhap) && s.MatKhau.Equals(pw)).ToList();
                if (data.Count() > 0)
                {
                    maKhachHang = data.FirstOrDefault().MaKhachHang;
                    KhachHang kh = db.KhachHang.SingleOrDefault(k => k.MaKhachHang == maKhachHang);
                    ViewBag.hoTen = kh.TenKhachHang;
                    ViewBag.diaChi = kh.DiaChi;
                    ViewBag.soDienThoai = kh.SoDienThoai;
                    ViewBag.email = kh.Email;
                    ViewBag.maKH = maKhachHang;
                    return RedirectToAction("ViewThanhToan");

                }
                else
                    return RedirectToAction("ViewThanhToan");
            }
            var gioHang = db.GioHang.Include(g => g.SanPham);
            gioHang = gioHang.OrderBy(g => g.MaSanPham);
            return View(gioHang.ToList());
        }

        //GET: View thanh toán thành công
        public ActionResult ViewThanhCong(string madh)
        {
            ViewBag.tongTien = GiaTriGioHang();
            ViewBag.maKHang = maKhachHang;

            if (maKhachHang != "")
            {
                Random ran = new Random();
                int maDH = ran.Next(1000, 9999);
                while (db.GioHang.SingleOrDefault(s => s.MaSanPham == maDH.ToString()) != null)
                {
                    maDH = ran.Next(1000, 9999);
                }

                string madonhang = "DH" + maDH.ToString();
                var gioHangs = db.GioHang.Include(g => g.SanPham);
                foreach (var item in gioHangs)
                {
                    DonHang dh = new DonHang();
                    dh.MaDonHang = madonhang;
                    dh.MaKhachHang = maKhachHang;
                    dh.MaSanPham = item.MaSanPham;
                    dh.SoLuong = item.SoLuong;
                    var tt = DateTime.Now.ToString("yyyy-MM-dd");
                    dh.ThoiGian = Convert.ToDateTime(tt);
                    dh.TinhTrang = false;
                    db.DonHang.Add(dh);
                    db.GioHang.Remove(item);
                }
                db.SaveChanges();

                var donhang = db.DonHang.Where(g => g.MaDonHang == madonhang);
                donhang = donhang.OrderBy(g => g.MaSanPham);
                ViewBag.tongTien = 0;
                return View(donhang.ToList());
            }
            else
            {
                return RedirectToAction("ViewThanhToan");
            }
        }

        public ActionResult ViewTaiKhoan()
        {
            ViewBag.tongTien = GiaTriGioHang();
            ViewBag.maKHang = maKhachHang;

            var donhang = db.DonHang.Where(d => d.MaKhachHang == maKhachHang);
            donhang = donhang.OrderByDescending(d => d.ThoiGian);
            return View(donhang.ToList());
        }

        //GET: View đăng ký
        public ActionResult ViewDangKy()
        {
            ViewBag.tongTien = GiaTriGioHang();
            ViewBag.maKHang = maKhachHang;

            return View();
        }

        //POST: View đăng ký
        [HttpPost]
        public ActionResult ViewDangKy(string tenDangNhap, string matKhau, string hoTen, string diaChi, string sdt, string email)
        {
            ViewBag.tongTien = GiaTriGioHang();
            ViewBag.maKHang = maKhachHang;

            if (ModelState.IsValid)
            {
                var checkEmail = db.KhachHang.FirstOrDefault(s => s.Email == email);
                var checkTenDN = db.KhachHang.FirstOrDefault(s => s.TenDangNhap == tenDangNhap);
                if (checkTenDN == null)
                {
                    if (checkEmail == null)
                    {
                        Random ran = new Random();
                        int maDH = ran.Next(1000, 9999);
                        while (db.GioHang.SingleOrDefault(s => s.MaSanPham == maDH.ToString()) != null)
                        {
                            maDH = ran.Next(1000, 9999);
                        }

                        string maKH = "KH" + maDH.ToString();

                        KhachHang kh = new KhachHang();
                        kh.MaKhachHang = maKH;
                        kh.TenDangNhap = tenDangNhap;
                        kh.MatKhau = GetMD5(matKhau);
                        kh.TenKhachHang = hoTen;
                        kh.DiaChi = diaChi;
                        kh.SoDienThoai = sdt;
                        kh.Email = email;
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.KhachHang.Add(kh);
                        db.SaveChanges();
                        return RedirectToAction("ViewDangNhap");
                    }
                    else
                    {
                        ViewBag.error = "Email đã tồn tại!";
                        return View();
                    }
                }
                else
                {
                    ViewBag.error = "Tên đăng nhập đã tồn tại!";
                    return View();
                }
            }
            return View();
        }


        //GET: View đăng nhập
        public ActionResult ViewDangNhap()
        {
            ViewBag.tongTien = GiaTriGioHang();
            ViewBag.maKHang = maKhachHang;

            return View();
        }

        //POST: View đăng nhập
        [HttpPost]
        public ActionResult ViewDangNhap(string tenDangNhap, string matKhau)
        {
            ViewBag.tongTien = GiaTriGioHang();
            ViewBag.maKHang = maKhachHang;

            if (ModelState.IsValid)
            {
                var pw = GetMD5(matKhau);
                var data = db.KhachHang.Where(s => s.TenDangNhap.Equals(tenDangNhap) && s.MatKhau.Equals(pw)).ToList();
                if (data.Count() > 0)
                {
                    Session["MaKhachHang"] = data.FirstOrDefault().MaKhachHang;
                    maKhachHang = data.FirstOrDefault().MaKhachHang;
                    return RedirectToAction("ViewTaiKhoan");

                }
                else
                {
                    ViewBag.error = "Đăng nhập không thành công!";
                    //return RedirectToAction("ViewDangNhap");
                    return View();
                }
            }
            return View();

        }

        //Đăng xuất tài khoản
        public ActionResult DangXuatTaiKhoan()
        {
            ViewBag.tongTien = GiaTriGioHang();
            ViewBag.maKHang = maKhachHang;

            Session.Clear();
            maKhachHang = "";
            return RedirectToAction("TrangChu");
        }


        //Tính toán giỏ hàng
        public int GiaTriGioHang()
        {
            var ghang = db.GioHang.Include(s => s.SanPham);
            int tongTien = 0;
            foreach (var item in ghang)
            {
                tongTien = tongTien + Convert.ToInt32(item.SanPham.GiaBan) * Convert.ToInt32(item.SoLuong);
            }
            return tongTien;
        }

        //Tạo phương thức mã hóa mật khẩu theo MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

        // GET: Test Session
        public ActionResult Index()
        {
            ViewBag.tongTien = GiaTriGioHang();
            ViewBag.maKHang = maKhachHang;

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}