namespace TradingCompany.DAL.DTOs
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public string SKU { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        
        public decimal UnitPrice { get; set; }
        public int StockQuantity {get; set; }    
        
        
    }
}