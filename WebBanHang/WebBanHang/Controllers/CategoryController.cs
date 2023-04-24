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
    public class CategoryController : Controller
    {
        // GET: Category
        TNSTOREEntities objTNSTOREEntities = new TNSTOREEntities();
        public ActionResult Index()
        {
            var lstringCategory = objTNSTOREEntities.Categories.ToList();
            return View(lstringCategory);
        }
        public ActionResult CategoryProduct(int Id, int? page)
        {
            Category obj = new Category();
            var lstCateProduct = objTNSTOREEntities.Products.Where(n => n.CategoryId == Id).ToList();
            int pagesize = 4;
            int PageNumber = (page ?? 1);
            ViewBag.Count = lstCateProduct.Count;
            return View(lstCateProduct.ToPagedList(PageNumber, pagesize));
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (objTNSTOREEntities != null)

                    objTNSTOREEntities.Dispose();
                objTNSTOREEntities.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}