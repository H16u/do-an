using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BTL_N9.Models;
using BTL_N9.Areas.Admin.MaHoa;
using PagedList;

namespace BTL_N9.Areas.Admin.Controllers
{
    public class KhachHangsController : Controller
    {
        private BTLDB db = new BTLDB();

        // GET: Admin/KhachHangs
        public ActionResult Index(string sortOrder, string seachString, string currentFilter, int? page)
        {
            var userSession = (UserLogin)Session[PLLogin.USER_SESSION];
            if (userSession == null)
            {
                return Redirect("~/Admin/Login/Index");
            }
            var khachHangs = db.KhachHang.Select(s => s);
            khachHangs = khachHangs.OrderBy(s => s.MaKhachHang);
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            

            //cac bien sx
            ViewBag.CurrentSort = sortOrder; //Biến lấy yêu cầu sắp xếp hiện tại

            ViewBag.SapTheoTen = String.IsNullOrEmpty(sortOrder) ? "ten_desc" : "";
            ViewBag.SapTheoDiaChi = sortOrder == "DiaChi" ? "DiaChi_desc" : "DiaChi";
            ViewBag.SapTheoMaKhachHang = sortOrder == "MaKhachHang" ? "MaKhachHang_desc" : "MaKhachHang";

            //Lấy giá trị của bộ lọc sắp xếp hiện tại
            if (seachString != null)
            {
                page = 1;
            }
            else
            {
                seachString = currentFilter;
            }
            ViewBag.CurrentFilter = seachString;

            //lay ds hang          
            
            //lọc theo tên hàng
            if (!String.IsNullOrEmpty(seachString))
            {
                khachHangs = khachHangs.Where(p => p.TenKhachHang.Contains(seachString));
            }
            //sx
            switch (sortOrder)
            {
                case "ten_desc":
                    khachHangs = khachHangs.OrderByDescending(s => s.TenKhachHang);
                    break;
                case "DiaChi":
                    khachHangs = khachHangs.OrderBy(s => s.DiaChi);
                    break;
                case "DiaChi_desc":
                    khachHangs = khachHangs.OrderByDescending(s => s.DiaChi);
                    break;
                case "MaKhachHang":
                    khachHangs = khachHangs.OrderBy(s => s.MaKhachHang);
                    break;
                case "MaKhachHang_desc":
                    khachHangs = khachHangs.OrderByDescending(s => s.MaKhachHang);
                    break;
                default:
                    khachHangs = khachHangs.OrderBy(s => s.TenKhachHang);
                    break;
            }          
            return View(khachHangs.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/KhachHangs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHang.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // GET: Admin/KhachHangs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/KhachHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaKhachHang,TenDangNhap,MatKhau,TenKhachHang,DiaChi,SoDienThoai,Email")] KhachHang khachHang)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.KhachHang.Add(khachHang);
                    db.SaveChanges();
                    
                }
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ViewBag.Error = "lỗi nhập dữ liệu!" + ex.Message;
                return View(khachHang);
            }
            

            
        }

        // GET: Admin/KhachHangs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHang.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // POST: Admin/KhachHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaKhachHang,TenDangNhap,MatKhau,TenKhachHang,DiaChi,SoDienThoai,Email")] KhachHang khachHang)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(khachHang).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }catch(Exception ex)
            {
                ViewBag.Error = "lỗi nhập dữ liệu!" + ex.Message;
                return View(khachHang);
            }
            
            
        }

        // GET: Admin/KhachHangs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHang.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // POST: Admin/KhachHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            KhachHang khachHang = db.KhachHang.Find(id);
            db.KhachHang.Remove(khachHang);
            db.SaveChanges();
            return RedirectToAction("Index");
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
