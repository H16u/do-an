using BTL_N9.Models;
using BTL_N9.Areas.Admin.MaHoa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTL_N9.Areas.Admin.Models
{
    public class Login
    {
        BTLDB db = new BTLDB();
        public Login()
        {
        }
        public int DangNhap(string name, string pass)
        {
            var taikhoan = db.NhanVien.SingleOrDefault(x => x.TenDangNhap == name);
            if (taikhoan == null)
            {
                return 0;
            }
            else
            {
                if (taikhoan.MaChucVu.Trim() == LoginSesion.ADMIN_SESSION.Trim() || taikhoan.MaChucVu.Trim() == LoginSesion.USER_SESSION)
                {
                    if (taikhoan.MatKhau == pass)
                        return 1;
                    else
                        return -2;
                }
                else
                {
                    return -3;
                }
            }

        }
        public int DNUser(string name, string pass)
        {
            var taikhoan = db.NhanVien.SingleOrDefault(x => x.TenDangNhap == name);
            if (taikhoan == null)
            {
                return 0;
            }
            else
            {
                if (taikhoan.MaChucVu.Trim() == LoginSesion.ADMIN_SESSION.Trim())
                {
                    if (taikhoan.MatKhau == pass)
                        return 1;
                    else
                        return -2;
                }
                else if(taikhoan.MaChucVu.Trim() == LoginSesion.USER_SESSION.Trim())
                {
                    return -3;
                }
                else
                {
                    return -3;
                }
            }
        }
        public NhanVien GetUserByName(string Name)
        {
            return db.NhanVien.SingleOrDefault(x => x.TenDangNhap == Name);
        }
    }
}