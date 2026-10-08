using System.Collections;
using System.Collections.Generic;

namespace TradingCompany.DAL.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public string SKU { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int StockQuantity { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual Supplier Supplier { get; set; } = null!;
        public virtual ICollection <Sale> Sales { get; set; } = new List <Sale>();
    }
}