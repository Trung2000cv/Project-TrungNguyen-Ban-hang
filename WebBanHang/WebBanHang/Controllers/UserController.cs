using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Context;
using System.Security.Cryptography;
using System.Text;

namespace WebBanHang.Controllers
{
    public class UserController : Controller
    {
        TNSTOREEntities db = new TNSTOREEntities();

        // GET: User
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                int id = int.Parse(Session["Id"].ToString());
                var user = db.Users.Where(a => a.Id == id).FirstOrDefault();
                return View(user);
            }
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Index(User user)
        {
             user.Id = (int)Session["Id"];
            
             db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            user.PassWord = GetMD5(user.PassWord);
             db.SaveChanges();
            return RedirectToAction("Login", "Home");
        }
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
    }
}