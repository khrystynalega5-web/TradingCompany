using System.Collections;
using System.Collections.Generic;

namespace TradingCompany.DAL.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection <Product> Products { get; set; } = new List<Product>();
    }
}
