using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Context;
using WebBanHang.Models;
using static WebBanHang.Common;
using PagedList.Mvc;
using PagedList;

namespace WebBanHang.Controllers
{
   
    public class TinhTrangGiaoHangController : Controller
    {
        TNSTOREEntities objTNStoreEntity = new TNSTOREEntities();
        // GET: TinhTrangGiaoHang
        public ActionResult Index(int? page)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                DonHang_ChiTietModel obj = new DonHang_ChiTietModel();
                int id = int.Parse(Session["Id"].ToString());
                int pagesize = 4;
                int PageNumber = (page ?? 1);
                var lisOrder = objTNStoreEntity.Orders.Where(n => n.UserId == id).OrderByDescending(n => n.Id).ToList();
                if (lisOrder.Count == 0)
                {
                    ViewBag.Tb = "Bạn chưa đặt hàng";
                }

                return View(lisOrder.ToPagedList(PageNumber, pagesize));

            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            this.LoadData();
            var obj = objTNStoreEntity.Orders.Where(n => n.Id == id).FirstOrDefault();
            return View(obj);
            
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(Order obj)
        {
            this.LoadData();
            obj.UserId = (int)Session["Id"];
            objTNStoreEntity.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            objTNStoreEntity.SaveChanges();
            return RedirectToAction("Index");
        }
        void LoadData()
        {
            Common objCommon = new Common();
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            List<Status> lstStatus = new List<Status>();
            Status objType = new Status();
            //nhận hàng
            objType.Id = 2;
            objType.Name = "Đã nhận hàng";
            lstStatus.Add(objType);
            DataTable dtProductType = converter.ToDataTable(lstStatus);
            ViewBag.ProductType = objCommon.ToSelectList(dtProductType, "Id", "Name");
        }
        //hủy đơn hàng 
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

            for (int i = 0; i < objOrderIdCount; i++)
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
            //var lstOrder = objTNStoreEntity.Orders.Where(a => a.Id == Id).FirstOrDefault();
            var lstCTDonHang = objTNStoreEntity.OrderDetails.Where(n => n.OrderId == Id).ToList();
            var tongtien = objTNStoreEntity.OrderDetails.Where(n => n.OrderId == Id).Sum(n => n.DonGia * n.Quantity);
            ViewBag.Sum = tongtien;
            //ViewBag.lstDonhang = lstCTDonHang;
            return View(lstCTDonHang);
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