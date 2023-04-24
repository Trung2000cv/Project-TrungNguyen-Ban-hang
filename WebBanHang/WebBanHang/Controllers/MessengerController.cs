using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Context;
using PagedList.Mvc;
using PagedList;

namespace WebBanHang.Controllers
{
    public class MessengerController : Controller
    {
        TNSTOREEntities objTNStoreEntity = new TNSTOREEntities();
        // GET: Messenger
        public ActionResult Index(int? page)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                int id = int.Parse(Session["Id"].ToString());
                var lisComment = objTNStoreEntity.Comments.Where(n => n.IdUser == id ).ToList();
                ViewBag.lsst = lisComment;
                int pagesize = 8;
                int PageNumber = (page ?? 1);
                return View(lisComment.ToPagedList(PageNumber, pagesize));
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