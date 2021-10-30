using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnDemandService.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public int Price { get; set; }

        public string ProductCategory { get; set; }
    }
}