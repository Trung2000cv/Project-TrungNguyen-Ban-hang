using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Context;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class HomeController : Controller
    {
        TNSTOREEntities objTNStoreEntity = new TNSTOREEntities();
        public ActionResult Index()
        {
            HomeModel objHomeModel = new HomeModel();
            objHomeModel.ListCategory = objTNStoreEntity.Categories.ToList();
            objHomeModel.ListProduct = objTNStoreEntity.Products.ToList();
            return View(objHomeModel);
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User _user)
        {

            if (ModelState.IsValid)
            {
                var check = objTNStoreEntity.Users.FirstOrDefault(s => s.Email == _user.Email);
                if (check == null)
                {
                    _user.PassWord = GetMD5(_user.PassWord);
                    objTNStoreEntity.Configuration.ValidateOnSaveEnabled = false;
                    objTNStoreEntity.Users.Add(_user);
                    objTNStoreEntity.SaveChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.error = "Email đã tồn tại";
                    return View();
                }


            }
            return View();

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
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password, bool isAdmin = false)
        {
            if (ModelState.IsValid)
            {


                var f_password = GetMD5(password);
                var data = objTNStoreEntity.Users.Where(s => s.Email.Equals(email) && s.PassWord.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["FullName"] = data.FirstOrDefault().FirstName + " " + data.FirstOrDefault().LastName;
                    Session["Email"] = data.FirstOrDefault().Email;
                    Session["Id"] = data.FirstOrDefault().Id;
                    Session["isAdmin"] = data.FirstOrDefault().IsAdmin;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Đăng nhập thất bại. Vui lòng kiểm tra tài khoản hoặc mật khẩu";
                    return RedirectToAction("Login");
                }
            }

            return View();
            
        }


        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        if (objTNStoreEntity != null)

        //            objTNStoreEntity.Dispose();
        //        objTNStoreEntity.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}