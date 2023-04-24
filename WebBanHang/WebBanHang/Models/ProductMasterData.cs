using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanHang.Models
{
    public partial class ProductMasterData
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập tên sản phẩm")]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }
        [Display(Name = "Hình đại diện")]       
        public string Avater { get; set; }
        [Display(Name = "Loại sản phẩm")]
        public Nullable<int> CategoryId { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống!")]
        [Display(Name = "Miêu tả ngắn")]
        public string ShortDes { get; set; }  
        [Display(Name = "Miêu tả")]
        public string FullDescription { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống!")]
        [Display(Name = "Giá gốc sản phẩm")]
        public Nullable<double> Price { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống!")]
        [Display(Name = "Giá ưu đãi")]
        public Nullable<double> PriceDiscount { get; set; }
        [Display(Name = "Loại")]
        public Nullable<int> TypeId { get; set; }
        public string Slug { get; set; }
        [Display(Name = "Thương hiệu")]
        public Nullable<int> BrandId { get; set; }
        [Display(Name = "Năm sản xuất")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Year { get; set; }
      
        [Display(Name = "Khối lượng")]
        public Nullable<double> Wight { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống!")]
        [Display(Name = "CPU")]
        public string CPU { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống!")]
        [Display(Name = "Ram")]
        public string RAM { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống!")]
        [Display(Name = "Màu sắc")]
        public string Color { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống!")]
        [Display(Name = "Hệ điều hành")]
        public string OS { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống!")]
        [Display(Name = "Độ phân giải")]
        public Nullable<int> Resolution { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống!")]
        [Display(Name = "Chất liệu")]
        public string Material { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống!")]
        [Display(Name = "Tốc độ")]
        public Nullable<double> Speed { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống!")]
        [Display(Name = "Pin")]
        public Nullable<int> Battery { get; set; }
     
        [Display(Name = "Viewer")]
        public Nullable<int> Viewr { get; set; }
        //[Required(ErrorMessage = "Không được bỏ trống!")]
        [Display(Name = "Đánh giá")]
        public Nullable<int> point { get; set; }
    }
}