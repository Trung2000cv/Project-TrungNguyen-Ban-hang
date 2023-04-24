using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Context;

namespace WebBanHang.Areas.Admin.Controllers
{
    public class AdminCategoryController : BaseController
    {
        TNSTOREEntities objTNStoreEntity = new TNSTOREEntities();
        // GET: Admin/AdminCategory
        public ActionResult Index()
        {
            var listCategory = objTNStoreEntity.Categories.ToList();
            return View(listCategory);
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