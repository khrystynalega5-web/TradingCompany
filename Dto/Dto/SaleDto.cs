using System;

namespace TradingCompany.DAL.DTOs
{
    public class SaleDto
    {
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal SalePrice { get; set; }
        public decimal? TotalSum { get; set; }
        public DateTime? SaleDate { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public string? Status { get; set; }
    }
}