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
    public class TimKiemController : Controller
    {
        TNSTOREEntities objTNStoreEntity = new TNSTOREEntities();
        // GET: TimKiem
        [HttpPost]
        public ActionResult Search(FormCollection f, int? page)
        {
            string SearchString = f["SearchString"].ToString();
            ViewBag.key = SearchString;
            List<Product> listProduct = objTNStoreEntity.Products.Where(n => n.Name.Contains(SearchString)).ToList();
            int pagesize = 8;
            int PageNumber = (page ?? 1);
            if (listProduct.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy sản phẩm nào";
                return View(objTNStoreEntity.Products.OrderBy(n => n.Name).ToPagedList(PageNumber, pagesize));
            }

            ViewBag.ThongBao = "Đã tìm thấy " + listProduct.Count + "sản phẩm";
            return View(listProduct.OrderBy(n => n.Name).ToPagedList(PageNumber, pagesize));
        }
        [HttpGet]
        public ActionResult Search(string SearchString, int? page)
        {
            ViewBag.key = SearchString;
            List<Product> listProduct = objTNStoreEntity.Products.Where(n => n.Name.Contains(SearchString)).ToList();
            int pagesize = 8;
            int PageNumber = (page ?? 1);
            if (listProduct.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy sản phẩm nào";
                return View(objTNStoreEntity.Products.OrderBy(n => n.Name).ToPagedList(PageNumber, pagesize));
            }

            ViewBag.ThongBao = "Đã tìm thấy " + listProduct.Count + "sản phẩm";
            return View(listProduct.OrderBy(n => n.Name).ToPagedList(PageNumber, pagesize));
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