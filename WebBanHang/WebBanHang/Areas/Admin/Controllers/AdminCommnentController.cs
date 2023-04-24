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
    public class AdminCommnentController : BaseController
    {
        TNSTOREEntities objTNStoreEntity = new TNSTOREEntities();
        // GET: Admin/AdminCommnent
        public ActionResult Index( int? page)
        {
            var lstComment = new List<Comment>();
             lstComment = objTNStoreEntity.Comments.ToList();
            lstComment = lstComment.OrderByDescending(n => n.Id).ToList();
            int pagesize = 4;
            int PageNumber = (page ?? 1);
            return View(lstComment.ToPagedList(PageNumber, pagesize));
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objProduct = objTNStoreEntity.Comments.Where(n => n.Id == id).FirstOrDefault();
            return View(objProduct);
           
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(Comment cmt)
        {
            cmt.Dayresquest = DateTime.Now;
            objTNStoreEntity.Entry(cmt).State = System.Data.Entity.EntityState.Modified;
            objTNStoreEntity.SaveChanges();
            return RedirectToAction("Index");
           
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