using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Context;
using WebBanHang.Models;
using PagedList.Mvc;
using PagedList;
using static WebBanHang.Common;
using System.Data;

namespace WebBanHang.Areas.Admin.Controllers
{

    public class AdminCartController : BaseController
    {
        TNSTOREEntities objTNStoreEntity = new TNSTOREEntities();
        // GET: Admin/AdminCart
        public ActionResult Index(string SearchString, string currentFilter, int? page, string ChoGiaoHang, string DaNhan, string XacNhan, string Huy)
        {
            //this.LoadData();
            var lstOrder = new List<Order>();
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
                lstOrder = objTNStoreEntity.Orders.Where(n => n.Name.Contains(SearchString)).ToList();

            }
            else
            {
                lstOrder = objTNStoreEntity.Orders.ToList();
            } 
            //các đơn chờ xác nhận
            if(ChoGiaoHang != null)
            {
                lstOrder = objTNStoreEntity.Orders.Where(n => n.Status == 1).ToList();

            }
          
            //các đơn đã nhận
            if(DaNhan != null)
            {
                lstOrder = objTNStoreEntity.Orders.Where(n => n.Status == 2).ToList();
            }
            //các đơn chờ giao hàng
            if (XacNhan != null)
            {
                lstOrder = objTNStoreEntity.Orders.Where(n => n.Status == 3).ToList();
            }
            if(Huy != null)
            {
                lstOrder = objTNStoreEntity.Orders.Where(n => n.Status == 4).ToList();
            }

            ViewBag.currentFilter = SearchString;
            int pagesize = 4;
            int PageNumber = (page ?? 1);

            lstOrder = lstOrder.OrderByDescending(n => n.Id).ToList();
            return View(lstOrder.ToPagedList(PageNumber, pagesize));
        
    }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            this.LoadData();
            var objProduct = objTNStoreEntity.Orders.Where(n => n.Id == id).FirstOrDefault();
            return View(objProduct);


        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(Order obj)
        {
            this.LoadData();
            objTNStoreEntity.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            objTNStoreEntity.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult HuyDonHang(int id)
        {
            var objProduct = objTNStoreEntity.Orders.Where(n => n.Id == id).FirstOrDefault();
            return View(objProduct);


        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult HuyDonHang(Order obj)
        {
            obj.Status = 4;//Gán trạng thái : Đã hủy đơn hàng
            objTNStoreEntity.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            objTNStoreEntity.SaveChanges();
            return RedirectToAction("Index");
        }
        // Xóa đơn hàng
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objOrder = objTNStoreEntity.Orders.Where(n => n.Id == id).FirstOrDefault();
            return View(objOrder);
        }
        [HttpPost]
        public ActionResult Delete(Order objPro)
        {
            var objOrder = objTNStoreEntity.Orders.Where(n => n.Id == objPro.Id).FirstOrDefault();
            var objOrderIdCount = objTNStoreEntity.OrderDetails.Where(s => s.OrderId == objPro.Id).Count();

            for(int i=0; i< objOrderIdCount; i++)
            {
               objTNStoreEntity.OrderDetails.Where(s => s.OrderId == objPro.Id).FirstOrDefault();
                objTNStoreEntity.OrderDetails.Remove(objTNStoreEntity.OrderDetails.Where(s => s.OrderId == objPro.Id).FirstOrDefault());
            }
            objTNStoreEntity.Orders.Remove(objOrder);
            objTNStoreEntity.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Details(int Id)
        {
           
            var lstCTDonHang = objTNStoreEntity.OrderDetails.Where(n => n.OrderId == Id).ToList();
            var tongtien = objTNStoreEntity.OrderDetails.Where(n => n.OrderId == Id).Sum(n => n.DonGia * n.Quantity);
            ViewBag.Sum = tongtien;
            return View(lstCTDonHang);
        }
        void LoadData()
        {
            Common objCommon = new Common();
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            List<Status> lstStatus = new List<Status>();
            Status objType = new Status();
       

            objType = new Status();
            objType.Id = 3;
            objType.Name = "Đang Giao Hàng";
            lstStatus.Add(objType);

            objType = new Status();
            objType.Id = 2;
            objType.Name = "Đã nhận hàng";
            lstStatus.Add(objType);

            DataTable dtProductType = converter.ToDataTable(lstStatus);
            ViewBag.ProductType = objCommon.ToSelectList(dtProductType, "Id", "Name");
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