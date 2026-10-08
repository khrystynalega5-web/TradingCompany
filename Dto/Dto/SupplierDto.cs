namespace TradingCompany.DAL.DTOs
{
    public class SupplierDto
    {
        public int SupplierId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string ContactPhone { get; set; } = null!;
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string City { get; set; } = null!;
    }
}