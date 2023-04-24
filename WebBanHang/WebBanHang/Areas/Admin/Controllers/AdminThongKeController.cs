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

    public class AdminThongKeController : Controller
    {
        TNSTOREEntities db = new TNSTOREEntities();
        // GET: Admin/AdminThongKe
        public ActionResult Index(DateTime? start, DateTime? end)
        { 
            Order_OrderDetailModel model = new Order_OrderDetailModel();
            //-----//

            //version end
            ViewBag.DoanhThu = DoanhThu();
            var query = db.OrderDetails.GroupBy(a => a.SanPham).Select(a => new { soluong = a.Sum(b => b.Quantity), name = a.Key }).ToList();//lấy số lượng theo sản phẩm
            ViewBag.tongSanPham = query.Sum(n => n.soluong);// tổng số sản phẩm
            var lstSanPham = query.OrderByDescending(n => n.soluong).ToList();// sắp xếp sản phẩm theo số lượng
            model.ListOrder = db.Orders.Where(a => a.CreatedOnUtc >= start && a.CreatedOnUtc <= end).ToList();//đơn hàng theo khoảng thời gian
            var sanPhamBanChay = lstSanPham.FirstOrDefault();// sản phẩm có số lượng đặt nhiều nhất
            ViewBag.slbc = sanPhamBanChay.soluong;//số lượng sản phẩm bán chạy nhất
            var lstSpMax = from kq in lstSanPham where kq.soluong == sanPhamBanChay.soluong select kq.name;//sắp xếp các tên có cùng số lượng max
            ViewBag.lstSpMax = lstSpMax.ToList();

            model.ListOrderDetail = db.OrderDetails.Where(a => a.NgayDatHang <= end && a.NgayDatHang >= start).ToList();//chi tiết các sản phẩm dược đặt trong khoảng thời gian
            var sanphamtheongay = model.ListOrderDetail.GroupBy(a => a.SanPham).Select(a => new { soluong = a.Sum(b => b.Quantity), name = a.Key }).ToList();
            var count = sanphamtheongay.Count;
            if (count != 0)
            {
                var Sort = sanphamtheongay.OrderByDescending(n => n.soluong).ToList();// sắp xếp danh sách sản phẩm theo số lượng
                ViewBag.slOfSP = Sort.FirstOrDefault().soluong;
                var SortSl = Sort.FirstOrDefault().soluong;
                var SortMax = from sp in Sort where sp.soluong == Sort.FirstOrDefault().soluong select sp.name;
                ViewBag.SortMax = SortMax.ToList();
            }
            //Danh sách đơn hàng đã thanh toán
            model.lstDonHang = db.Orders.Where(n => n.CreatedOnUtc <= end && n.CreatedOnUtc >= start && n.Status == 2).ToList();
            ViewBag.OrderDone = model.lstDonHang.Count;
            ViewBag.sosp = model.ListOrderDetail.Sum(n => n.Quantity);//số sản phẩm

            ViewBag.sumTien = model.ListOrderDetail.Sum(n => n.Quantity * n.DonGia);// Tiền sản phẩm bán theo ngày
            ViewBag.sodh = model.ListOrder.Count;
            // sản phảm đã thanh toaqns
            model.DaThanhToan = (from a in model.lstDonHang join b in model.ListOrderDetail on a.Id equals b.OrderId select b).ToList();
            ViewBag.soluong = Sodonhang();
            ViewBag.songuoidung = Songuoidung();

            ViewBag.start = start;
            ViewBag.end = end;

            ViewBag.DoanhThuTheoThang = DoanhThuTheoThang(start, end);
            return View(model);

        }
        public decimal DoanhThu()
        {
            decimal tongDoanhThu = (decimal)db.OrderDetails.Sum(n => n.DonGia * n.Quantity).Value;
            return tongDoanhThu;

        }
        public decimal DoanhThuTheoThang(DateTime? start, DateTime? end)
        {
            var lstDonHang = db.Orders.Where(n => n.CreatedOnUtc <= end && n.CreatedOnUtc >=  start && n.Status == 2);
            decimal Money = 0;
            foreach (var item in lstDonHang)
            {
                Money += decimal.Parse(item.OrderDetails.Sum(n => n.DonGia * n.Quantity).Value.ToString());
            }
            return Money;

        }
      public decimal sumMoney() {
            
                var lstDonHang = db.Orders.Where(n => n.Status == 2);
                decimal Money = 0;
                foreach (var item in lstDonHang)
                {
                    Money += decimal.Parse(item.OrderDetails.Sum(n => n.DonGia * n.Quantity).Value.ToString());
                }
                return Money;
            
        }
        public int Sodonhang()
        {
            var soluong = db.Orders.Where(a => a.Status ==2).ToList().Count();
            return soluong;
        }
        public int Songuoidung()
        {
            var soluong = db.Users.ToList().Count();
            return soluong;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)

                    db.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}