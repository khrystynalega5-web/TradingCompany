using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingCompany.DAL.Models
{
    public class Sale
    {
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal SalePrice { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? TotalSum { get; set; }

        public DateTime? SaleDate { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public string? Status { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}