using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTL_N9.Models;
using BTL_N9.Areas.Admin.MaHoa;
using BTL_N9.Areas.Admin.Models;

namespace BTL_N9.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        [HttpGet]
        public ActionResult Index()
        {
            NhanVien tk = new NhanVien();
            return View(tk);
        }
        [HttpPost]
        public ActionResult Index(NhanVien tk)
        {
            ViewBag.ErrorMessage = "";

            Login login = new Login();
            var result = login.DangNhap(tk.TenDangNhap.Trim().ToString(), tk.MatKhau.Trim().ToString());
            if (result == 1)
            {
                var user = login.GetUserByName(tk.TenDangNhap);
                var userSession = new UserLogin();
                userSession.MatKhau = user.MatKhau;
                userSession.TenDangNhap = user.TenDangNhap;
                userSession.MaChucVu = user.MaChucVu;
                Session.Add(PLLogin.USER_SESSION, userSession);
                return Redirect("~/Admin/Home/Index");
            }
            else if (result == 0)
            {
                ViewBag.ErrorMessage = "Tài khoản không tồn tại";
            }
            else if (result == -2)
            {
                ViewBag.ErrorMessage = "Mật khẩu không đúng";
            }
            else if (result == -3)
            {
                ViewBag.ErrorMessage = "Tài khoản của bạn không có quyền đăng nhập";
            }
            else
            {
                ViewBag.ErrorMessage = "Đăng nhập không thành công!";
            }
            return View(tk);
        }
        [HttpGet]
        public ActionResult LoginQLUser()
        {
            NhanVien nv = new NhanVien();
            return View(nv);
        }

        [HttpPost]
        public ActionResult LoginQLUser(NhanVien tk)
        {
            ViewBag.ErrorMessage = "";

            Login login = new Login();
            var result = login.DNUser(tk.TenDangNhap.Trim().ToString(), tk.MatKhau.Trim().ToString());
            if (result == 1)
            {
                var user = login.GetUserByName(tk.TenDangNhap);
                var userSession = new UserLogin();
                userSession.MatKhau = user.MatKhau;
                userSession.TenDangNhap = user.TenDangNhap;
                userSession.MaChucVu = user.MaChucVu;
                Session.Add(PLLogin.USER_SESSION, userSession);
                return Redirect("~/Admin/NhanViens/Index");
            }
            else if (result == 0)
            {
                ViewBag.ErrorMessage = "Tài khoản không tồn tại";
            }
            else if (result == -2)
            {
                ViewBag.ErrorMessage = "Mật khẩu không đúng";
            }
            else if (result == -3)
            {
                ViewBag.ErrorMessage = "Tài khoản của bạn không có quyền đăng nhập";
            }
            else
            {
                ViewBag.ErrorMessage = "Đăng nhập không thành công!";
            }
            return View(tk);
        }

        //public ActionResult DangXuatTaiKhoan()
        //{
        //    var ghang = db.GioHang.Include(s => s.SanPham);
        //    int tongTien = 0;
        //    foreach (var item in ghang)
        //    {
        //        tongTien = tongTien + Convert.ToInt32(item.SanPham.GiaBan) * Convert.ToInt32(item.SoLuong);
        //    }
        //    ViewBag.tongTien = tongTien;

        //    Session.Clear();
        //    maKhachHang = null;
        //    return RedirectToAction("ViewDangNhap");
        //}

    }
}