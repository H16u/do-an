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
    public class ThuongHieuxController : Controller
    {
        private BTLDB db = new BTLDB();

        // GET: Admin/ThuongHieux
        public ActionResult Index(string sortOrder, string seachString, string currentFilter, int? page)
        {
            var userSession = (UserLogin)Session[PLLogin.USER_SESSION];
            if (userSession == null)
            {
                return Redirect("~/Admin/Login/Index");
            }
            var ThuongHieux = db.ThuongHieu.Select(s => s);
            ThuongHieux = ThuongHieux.OrderBy(s => s.MaThuongHieu);
            int pageSize = 6;
            int pageNumber = (page ?? 1);           

            //cac bien sx
            ViewBag.CurrentSort = sortOrder; //Biến lấy yêu cầu sắp xếp hiện tại

            ViewBag.SapTheoTen = String.IsNullOrEmpty(sortOrder) ? "ten_desc" : "";
            ViewBag.SapTheoMaThuongHieu = sortOrder == "MaThuongHieu" ? "MaThuongHieu_desc" : "MaThuongHieu";

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
                ThuongHieux = ThuongHieux.Where(p => p.TenThuongHieu.Contains(seachString));
            }
            //sx
            switch (sortOrder)
            {
                case "ten_desc":
                    ThuongHieux = ThuongHieux.OrderByDescending(s => s.TenThuongHieu);
                    break;
                case "MaThuongHieu":
                    ThuongHieux = ThuongHieux.OrderBy(s => s.MaThuongHieu);
                    break;
                case "MaThuongHieu_desc":
                    ThuongHieux = ThuongHieux.OrderByDescending(s => s.MaThuongHieu);
                    break;
                default:
                    ThuongHieux = ThuongHieux.OrderBy(s => s.TenThuongHieu);
                    break;
            }
            return View(ThuongHieux.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/ThuongHieux/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThuongHieu thuongHieu = db.ThuongHieu.Find(id);
            if (thuongHieu == null)
            {
                return HttpNotFound();
            }
            return View(thuongHieu);
        }

        // GET: Admin/ThuongHieux/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/ThuongHieux/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaThuongHieu,TenThuongHieu")] ThuongHieu thuongHieu)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.ThuongHieu.Add(thuongHieu);
                    db.SaveChanges();
                    
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "lỗi nhập dữ liệu!" + ex.Message;
                return View(thuongHieu);
            } 
        }

        // GET: Admin/ThuongHieux/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThuongHieu thuongHieu = db.ThuongHieu.Find(id);
            if (thuongHieu == null)
            {
                return HttpNotFound();
            }
            return View(thuongHieu);
        }

        // POST: Admin/ThuongHieux/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaThuongHieu,TenThuongHieu")] ThuongHieu thuongHieu)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(thuongHieu).State = EntityState.Modified;
                    db.SaveChanges();
                    
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "lỗi nhập dữ liệu!" + ex.Message;
                return View(thuongHieu);
            }
            
            
        }

        // GET: Admin/ThuongHieux/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThuongHieu thuongHieu = db.ThuongHieu.Find(id);
            if (thuongHieu == null)
            {
                return HttpNotFound();
            }
            return View(thuongHieu);
        }

        // POST: Admin/ThuongHieux/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ThuongHieu thuongHieu = db.ThuongHieu.Find(id);
            db.ThuongHieu.Remove(thuongHieu);
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
