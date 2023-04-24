using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Context;
using PagedList.Mvc;
using PagedList;
using System.IO;

namespace WebBanHang.Areas.Admin.Controllers
{
    public class AdminBrandController : BaseController
    {
        TNSTOREEntities objTNStoreEntity = new TNSTOREEntities();
        // GET: Admin/AdminBrand
        public ActionResult Index(string SearchString, string currentFilter, int? page)
        {
            //var listBrand = objTNStoreEntity.Brands.ToList();
            var lstBrand = new List<Brand>();
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            if (!String.IsNullOrEmpty(SearchString))
            {
                lstBrand = objTNStoreEntity.Brands.Where(n => n.Name.Contains(SearchString)).ToList();

            }
            else
            {
                lstBrand = objTNStoreEntity.Brands.ToList();
            }
            ViewBag.currentFilter = SearchString;
            int pagesize = 4;
            int PageNumber = (page ?? 1);
            return View(lstBrand.ToPagedList(PageNumber, pagesize));
        }
        public ActionResult Error()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(Brand objBrand)
        {
           if(ModelState.IsValid)
            {
                try
                {
                    if (objBrand.ImageUpload != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(objBrand.ImageUpload.FileName);
                        string extention = Path.GetExtension(objBrand.ImageUpload.FileName);
                        fileName = fileName + extention;
                        objBrand.Avatar = fileName;
                        objBrand.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items/"), fileName));
                    }
                    objTNStoreEntity.Brands.Add(objBrand);
                    objTNStoreEntity.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            return View(objBrand);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objBrand = objTNStoreEntity.Brands.Where(n => n.Id == id).FirstOrDefault();
            return View(objBrand);
        }
        [HttpPost]
        public ActionResult Delete(Brand obj)
        {
            try
            {
                var objBrand = objTNStoreEntity.Brands.Where(n => n.Id == obj.Id).FirstOrDefault();
                objTNStoreEntity.Brands.Remove(objBrand);
                objTNStoreEntity.SaveChanges();
                return RedirectToAction("Index");
            }catch(Exception error)
            {
                ViewBag.error = "Không thể xóa" + error.Message;
                return RedirectToAction("Error");
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
          
            var objBrand = objTNStoreEntity.Brands.Where(n => n.Id == id).FirstOrDefault();
            return View(objBrand);


        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(Brand obj)
        {
       
            if (obj.ImageUpload != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(obj.ImageUpload.FileName);
                string extention = Path.GetExtension(obj.ImageUpload.FileName);
                fileName = fileName + extention;
                obj.Avatar = fileName;
                obj.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items/"), fileName));
            }
            objTNStoreEntity.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            objTNStoreEntity.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Details(int Id)
        {
            var obj = objTNStoreEntity.Brands.Where(n => n.Id == Id).FirstOrDefault();
            return View(obj);
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