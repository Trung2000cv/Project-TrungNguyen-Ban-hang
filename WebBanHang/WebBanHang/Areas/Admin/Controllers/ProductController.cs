using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Context;
using WebBanHang.Models;
using static WebBanHang.Common;
using PagedList.Mvc;
using PagedList;

namespace WebBanHang.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Admin/Product
        TNSTOREEntities objTNStoreEntity = new TNSTOREEntities();
        public ActionResult Index(string SearchString, string currentFilter, int? page)
        {
            var lstProduct = new List<Product>();
            if(SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            if (!String.IsNullOrEmpty(SearchString))
                {
                    lstProduct = objTNStoreEntity.Products .Where(n => n.Name.Contains(SearchString)).ToList();

                }
            else
            {
                lstProduct = objTNStoreEntity.Products.ToList();
            }
            ViewBag.currentFilter = SearchString;
            int pagesize = 4;
            int PageNumber = (page ?? 1);

            lstProduct = lstProduct.OrderByDescending(n => n.Id).ToList() ;

                return View(lstProduct.ToPagedList(PageNumber,pagesize));
        }
        public ActionResult Details(int Id)
        {
            var objProduct = objTNStoreEntity.Products.Where(n => n.Id == Id).FirstOrDefault();
            var idBrand = objProduct.BrandId;
            var lstBrand = objTNStoreEntity.Brands.ToList();
            var Name = objTNStoreEntity.Brands.Where(n => n.Id == idBrand).FirstOrDefault();
            var NameBrand = Name.Name;
            ViewBag.Name = NameBrand;
            return View(objProduct);
        }
        [HttpGet]
        public ActionResult Create()
        {
            this.LoadData();
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(Product objProduct)
        {
            this.LoadData();
            if (ModelState.IsValid)
            {
                try
                {
                    if (objProduct.ImageUpload != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpload.FileName);
                        string extention = Path.GetExtension(objProduct.ImageUpload.FileName);
                        fileName = fileName + extention;
                        objProduct.Avater = fileName;
                        objProduct.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items/"), fileName));
                    }
                    objTNStoreEntity.Products.Add(objProduct);
                    objTNStoreEntity.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            return View(objProduct);
        }
        void LoadData()
        {
            Common objCommon = new Common();
            //Lấy dữ liệu danh mục 
            var listCategory = objTNStoreEntity.Categories.ToList();
            //convert sang dạng seleclist dạng value, text
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            DataTable dtCategory = converter.ToDataTable(listCategory);
            ViewBag.ListCategory = objCommon.ToSelectList(dtCategory, "Id", "Name");

            //Lấy dữ liệu dưới DB
            var lstBrand = objTNStoreEntity.Brands.ToList();
            //convert sang dạng seleclist dạng value, text
            DataTable dtBrand = converter.ToDataTable(lstBrand);

            ViewBag.ListBrand = objCommon.ToSelectList(dtBrand, "Id", "Name");

            //Loại sản phẩm
            List<ProductType> lstProductType = new List<ProductType>();
            ProductType objProductType = new ProductType();
            objProductType.Id = 1;
            objProductType.Name = "Giảm giá sốc";
            lstProductType.Add(objProductType);

            objProductType = new ProductType();
            objProductType.Id = 2;
            objProductType.Name = "Đề xuất";
            lstProductType.Add(objProductType);

            DataTable dtProductType = converter.ToDataTable(lstProductType);
            ViewBag.ProductType = objCommon.ToSelectList(dtProductType, "Id", "Name");
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objProduct = objTNStoreEntity.Products.Where(n => n.Id == id).FirstOrDefault();
            return View(objProduct);
        }
        [HttpPost]
        public ActionResult Delete(Product objPro)
        {
            //lấy sản phẩm có 
            var objProduct = objTNStoreEntity.Products.Where(n => n.Id == objPro.Id).FirstOrDefault();
            //xóa sản phẩm
            objTNStoreEntity.Products.Remove(objProduct);
            // đơn hàng có sản phẩm
            var lstOrderDetals = objTNStoreEntity.OrderDetails.Where(s => s.ProductId == objPro.Id).ToList();
          //xóa danh sách đơn hàng có sản phẩm trên và lưu lại
            objTNStoreEntity.OrderDetails.RemoveRange(lstOrderDetals);
            objTNStoreEntity.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            this.LoadData();
            var objProduct = objTNStoreEntity.Products.Where(n => n.Id == id).FirstOrDefault();
            return View(objProduct);
           
        
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(Product obj)
        {
            this.LoadData();
            if (obj.ImageUpload != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(obj.ImageUpload.FileName);
                string extention = Path.GetExtension(obj.ImageUpload.FileName);
                fileName = fileName + extention;
                obj.Avater = fileName;
                obj.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items/"), fileName));
            }
            try
            {
                objTNStoreEntity.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                objTNStoreEntity.SaveChanges();
            }catch(Exception ex)
            {
                ViewBag.erEdit = "Không thể xóa" + ex.Message;
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        if(objTNStoreEntity != null)
                
        //            objTNStoreEntity.Dispose();
        //        objTNStoreEntity.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}