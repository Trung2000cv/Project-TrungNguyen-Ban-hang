using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanHang.Context;

namespace WebBanHang.Models
{
    public class Order_OrderDetailModel
    {
        public List<Order> ListOrder { get; set; }
        public List<OrderDetail> ListOrderDetail { get; set; }

        public List<OrderDetail> DaThanhToan { get; set; }
        public List<Order> lstDonHang { get; set; }
    }
}