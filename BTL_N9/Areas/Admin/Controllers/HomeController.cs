using BTL_N9.Models;
using BTL_N9.Areas.Admin.MaHoa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTL_N9.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private BTLDB db = new BTLDB();
        // GET: Admin/Home
        public ActionResult Index()
        {
            var userSession = (UserLogin)Session[PLLogin.USER_SESSION];
            if (userSession == null)
            {
                return Redirect("~/Admin/Login/Index");
            }
            var sanPhams = db.SanPham.OrderByDescending(x => (x.SoLuongNhap - x.SoLuongTonKho));
            return View(sanPhams.ToList());

        }

        public ActionResult Logout()
        {
            Session["USER_SESSION"] = null;
            return Redirect("~/Admin/Login/Index");
        }
        
    }
}