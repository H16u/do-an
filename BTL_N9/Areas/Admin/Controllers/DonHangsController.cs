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
    public class DonHangsController : Controller
    {
        private BTLDB db = new BTLDB();

        // GET: Admin/DonHangs
        public ActionResult Index(int? page)
        {
            var userSession = (UserLogin)Session[PLLogin.USER_SESSION];
            if (userSession == null)
            {
                return Redirect("~/Admin/Login/Index");
            }
            var donHangs = db.DonHang.Include(d => d.KhachHang).Include(d => d.SanPham);
            donHangs = donHangs.OrderBy(s => s.TinhTrang);
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(donHangs.ToPagedList(pageNumber, pageSize));
           
        }
        
        [HttpPost]
        public ActionResult Index(string maDH, string maSP)
        {
            DonHang donHang = db.DonHang.FirstOrDefault(s => s.MaDonHang == maDH && s.MaSanPham == maSP);
            if (ModelState.IsValid)
            {
                SanPham sp = db.SanPham.FirstOrDefault(p => p.MaSanPham == maSP);
                if (donHang.TinhTrang == false)
                {
                    donHang.TinhTrang = true;
                    sp.SoLuongTonKho = sp.SoLuongTonKho - donHang.SoLuong;
                }
                else
                {
                    donHang.TinhTrang = false;
                    sp.SoLuongTonKho = sp.SoLuongTonKho + donHang.SoLuong;
                }
                db.Entry(sp).State = EntityState.Modified;
                db.Entry(donHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaKhachHang = new SelectList(db.KhachHang, "MaKhachHang", "TenDangNhap", donHang.MaKhachHang);
            ViewBag.MaSanPham = new SelectList(db.SanPham, "MaSanPham", "TenSanPham", donHang.MaSanPham);
            return RedirectToAction("Index");
        }
        


        // GET: Admin/DonHangs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHang.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }

        // POST: Admin/DonHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DonHang donHang = db.DonHang.Find(id);
            db.DonHang.Remove(donHang);
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
