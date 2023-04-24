using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Context;
using WebBanHang.Models;
using static WebBanHang.Common;

namespace WebBanHang.Controllers
{
    public class ThanhToanController : Controller
    {
        TNSTOREEntities objTNStoreEntity = new TNSTOREEntities();
        // GET: ThanhToan
        public ActionResult Index()
        {
    
            if (Session["Id"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                //Lấy danh sách đơn hàng từ session
                var lstCart = (List<CartModel>)Session["cart"];
                if(lstCart == null) {
                    return RedirectToAction("NotCart");

                }
                else
                {
                    
                    Order objOrder = new Order();
                    objOrder.Name = "DonHang" + DateTime.Now.ToString("yyyyMMddhhmmss");
                    objOrder.UserId = int.Parse(Session["Id"].ToString());
                    objOrder.CreatedOnUtc = DateTime.Now;
                    objOrder.Status = 1;
                    objTNStoreEntity.Orders.Add(objOrder);
                    objTNStoreEntity.SaveChanges();
                    // Lấy orderId vừa tạo lưu vào bảng orderdetail
                    int intOrderId = objOrder.Id;

                    List<OrderDetail> lstOrderDetail = new List<OrderDetail>();
                    foreach (var item in lstCart)
                    {
                        OrderDetail obj = new OrderDetail();
                        obj.Quantity = item.Quantity;
                        obj.OrderId = intOrderId;
                        obj.ProductId = item.Product.Id;
                        obj.NgayDatHang = objOrder.CreatedOnUtc;
                        obj.SanPham = item.Product.Name;
                        obj.DonGia = item.Product.PriceDiscount;
                        lstOrderDetail.Add(obj);
                    }
                    objTNStoreEntity.OrderDetails.AddRange(lstOrderDetail);
                    objTNStoreEntity.SaveChanges();
                    lstCart.Clear();
                 
                }
                Session["count"] = 0;
                return View();
            }
              
              
        }
        public ActionResult NotCart()
        {
            return View();
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