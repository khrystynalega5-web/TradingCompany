using System.Collections;
using System.Collections.Generic;

namespace TradingCompany.DAL.Models
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string ContactPhone { get; set; } = null!;
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string City { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}