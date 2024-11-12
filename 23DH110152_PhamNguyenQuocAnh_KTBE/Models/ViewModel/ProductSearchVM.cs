using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using PagedList.Mvc;

namespace _23DH110152_PhamNguyenQuocAnh_KTBE.Models.ViewModel
{
    public class ProductSearchVM
    {
        public string SearchTerm { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string SortOrder { get; set; }
        public int PageNumber { get; set; }//trang hiện tại
        public int PageSize { get; set; } = 10;//số sản phẩm mỗi trang
        public PagedList.IPagedList<Product> Products { get; set; }
    }
}