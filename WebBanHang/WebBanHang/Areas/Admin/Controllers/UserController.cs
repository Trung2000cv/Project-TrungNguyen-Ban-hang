using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Context;
using PagedList.Mvc;
using PagedList;

namespace WebBanHang.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        // GET: Admin/User
        TNSTOREEntities objTNStoreEntity = new TNSTOREEntities();
        public ActionResult Index(string SearchString, string currentFilter, int? page)
        {
            var listUser = objTNStoreEntity.Users.ToList();
            int pagesize = 4;
            int PageNumber = (page ?? 1);
            return View(listUser.ToPagedList(PageNumber, pagesize));

        }
        public ActionResult Details(int Id)
        {
            var obj = objTNStoreEntity.Users.Where(n => n.Id == Id).FirstOrDefault();
            return View(obj);
        }
        public ActionResult Error()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objUser = objTNStoreEntity.Users.Where(n => n.Id == id).FirstOrDefault();
            return View(objUser);
        }
        [HttpPost]
        public ActionResult Delete(User user)
        {
            try
            {
                var obj = objTNStoreEntity.Users.Where(n => n.Id == user.Id).FirstOrDefault();
                objTNStoreEntity.Users.Remove(obj);
                objTNStoreEntity.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception error)
            {
                ViewBag.error = "Không thể xóa" + error.Message;
                return RedirectToAction("Error");
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (objTNStoreEntity != null)

                    objTNStoreEntity.Dispose();
                objTNStoreEntity.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}