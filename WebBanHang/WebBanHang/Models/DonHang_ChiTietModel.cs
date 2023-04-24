using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanHang.Context;

namespace WebBanHang.Models
{
    public class DonHang_ChiTietModel
    {
        public List<OrderDetail> ChiTiet { get; set; }
        public List<Order> order { get; set; }
    }
}