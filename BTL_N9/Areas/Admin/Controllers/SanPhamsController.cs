using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BTL_N9.Models;
using BTL_N9.Areas.Admin.MaHoa;
using PagedList;

namespace BTL_N9.Areas.Admin.Controllers
{
    public class SanPhamsController : Controller
    {
        private BTLDB db = new BTLDB();

        // GET: Admin/SanPhams
        public ActionResult Index(string sortOrder, string seachString, string currentFilter, int? page)
        {
            var userSession = (UserLogin)Session[PLLogin.USER_SESSION];
            if (userSession == null)
            {
                return Redirect("~/Admin/Login/Index");
            }
            var sanPhams = db.SanPham.Include(s => s.ThuongHieu1);
            sanPhams = sanPhams.OrderByDescending(s => s.SoLuongNhap).OrderBy(s => s.SoLuongTonKho);
            int pageSize = 8;
            int pageNumber = (page ?? 1);

            //cac bien sx
            ViewBag.CurrentSort = sortOrder; //Biến lấy yêu cầu sắp xếp hiện tại

            ViewBag.SapTheoTen = String.IsNullOrEmpty(sortOrder) ? "ten_desc" : "";
            ViewBag.SapTheoGiaBan = sortOrder == "GiaBan" ? "GiaBan_desc" : "GiaBan";

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
                sanPhams = sanPhams.Where(p => p.TenSanPham.Contains(seachString));
            }
            //sx
            switch (sortOrder)
            {
                case "ten_desc":
                    sanPhams = sanPhams.OrderByDescending(s => s.TenSanPham);
                    break;
                case "GiaBan":
                    sanPhams = sanPhams.OrderBy(s => s.GiaBan);
                    break;
                case "GiaBan_desc":
                    sanPhams = sanPhams.OrderByDescending(s => s.GiaBan);
                    break;
                default:
                    sanPhams = sanPhams.OrderBy(s => s.TenSanPham);
                    break;
            }
            return View(sanPhams.ToPagedList(pageNumber, pageSize));

        }

        // GET: Admin/SanPhams/Details/5
        public ActionResult Details(string id)
        {
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

        // GET: Admin/SanPhams/Create
        public ActionResult Create()
        {
            ViewBag.MaThuongHieu = new SelectList(db.ThuongHieu, "MaThuongHieu", "MaThuongHieu");
            SanPham sp = new SanPham();
            return View(sp);
        }

        // POST: Admin/SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSanPham,TenSanPham,MaThuongHieu,TomTat,Anh,SoLuongNhap,SoLuongTonKho,GiaNhap,GiaDeXuat,GiaBan,BaoHiem,BaoHanh,ThamDinhThatGia,GiaoHang,ThuongHieu,XuatXu,DHDanhCho,KieuMay,KichCo,ChatLieuVo,ChatLieuDay,ChatLieuKinh,ChucNang,DoChiuNuoc")] SanPham sanPham)
        {
            try
            {
                ViewBag.MaThuongHieu = new SelectList(db.ThuongHieu, "MaThuongHieu", "MaThuongHieu", sanPham.MaThuongHieu);
                if (ModelState.IsValid)
                {
                    sanPham.Anh = "";
                    var f = Request.Files["ImageFile"];
                    if (f != null && f.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f.FileName);
                        string UploadPath = Server.MapPath("~/Content/fileimage/" + FileName);
                        f.SaveAs(UploadPath);
                        sanPham.Anh = FileName;
                    }
                    db.SanPham.Add(sanPham);
                    db.SaveChanges();
                    
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "lỗi nhập dữ liệu!" + ex.Message;
                return View(sanPham);
            }
        }

        // GET: Admin/SanPhams/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPham.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaThuongHieu = new SelectList(db.ThuongHieu, "MaThuongHieu", "MaThuongHieu", sanPham.MaThuongHieu);
            return View(sanPham);
        }

        // POST: Admin/SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSanPham,TenSanPham,MaThuongHieu,TomTat,Anh,SoLuongNhap,SoLuongTonKho,GiaNhap,GiaDeXuat,GiaBan,BaoHiem,BaoHanh,ThamDinhThatGia,GiaoHang,ThuongHieu,XuatXu,DHDanhCho,KieuMay,KichCo,ChatLieuVo,ChatLieuDay,ChatLieuKinh,ChucNang,DoChiuNuoc")] SanPham sanPham)
        {
            try
            {
                ViewBag.MaThuongHieu = new SelectList(db.ThuongHieu, "MaThuongHieu", "MaThuongHieu", sanPham.MaThuongHieu);
                if (ModelState.IsValid)
                {
                    sanPham.Anh = "";
                    var f = Request.Files["ImageFile"];
                    if (f != null && f.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f.FileName);
                        string UploadPath = Server.MapPath("~/Content/fileimage/" + FileName);
                        f.SaveAs(UploadPath);
                        sanPham.Anh = FileName;
                    }
                    db.Entry(sanPham).State = EntityState.Modified;
                    db.SaveChanges();
                    
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "lỗi nhập dữ liệu!" + ex.Message;
                return View(sanPham);
            }
        }

        // GET: Admin/SanPhams/Delete/5
        public ActionResult Delete(string id)
        {
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

        // POST: Admin/SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SanPham sanPham = db.SanPham.Find(id);
            db.SanPham.Remove(sanPham);
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
