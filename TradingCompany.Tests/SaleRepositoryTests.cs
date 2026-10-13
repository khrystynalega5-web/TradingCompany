using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using TradingCompany.DAL.Models;
using TradingCompany.DAL.Repositories;

namespace TradingCompany.Tests
{
    [TestFixture]
    public class SaleRepositoryTests
    {
        private TradingCompanyDbContext _context = null!;
        private SaleRepository _repository = null!;
        private Product _defaultProduct = null!;
        private Category _defaultCategory = null!;
        private Supplier _defaultSupplier = null!;

        [SetUp]
        public async Task SetUp()
        {
            var options = new DbContextOptionsBuilder<TradingCompanyDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TradingCompanyDbContext(options);
            _repository = new SaleRepository(_context);

            _defaultCategory = new Category { CategoryName = "Тестова категорія" };
            _defaultSupplier = new Supplier { CompanyName = "Тестовий постачальник", City = "Львів", ContactPhone = "+380000000000" };

            _context.Categories.Add(_defaultCategory);
            _context.Suppliers.Add(_defaultSupplier);
            await _context.SaveChangesAsync();

            _defaultProduct = new Product
            {
                ProductName = "Тестовий товар",
                SKU = "TEST-SKU-SALE",
                UnitPrice = 100m,
                StockQuantity = 10,
                CategoryId = _defaultCategory.CategoryId,
                SupplierId = _defaultSupplier.SupplierId
            };

            _context.Products.Add(_defaultProduct);
            await _context.SaveChangesAsync();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task AddAsync_ShouldAddSale()
        {
            var sale = new Sale
            {
                ProductId = _defaultProduct.ProductId,
                Quantity = 2,
                SalePrice = 100m,
                PaymentMethod = "Готівка"
            };

            await _repository.AddAsync(sale);
            var result = await _repository.GetByIdAsync(sale.SaleId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Quantity, Is.EqualTo(2));
            Assert.That(result.ProductId, Is.EqualTo(_defaultProduct.ProductId));
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllSales()
        {
            await _repository.AddAsync(new Sale { ProductId = _defaultProduct.ProductId, Quantity = 1, SalePrice = 50m, PaymentMethod = "Картка" });
            await _repository.AddAsync(new Sale { ProductId = _defaultProduct.ProductId, Quantity = 3, SalePrice = 50m, PaymentMethod = "Готівка" });

            var list = await _repository.GetAllAsync();

            Assert.That(list.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateSale()
        {
            var sale = new Sale { ProductId = _defaultProduct.ProductId, Quantity = 1, SalePrice = 50m, PaymentMethod = "Готівка" };
            await _repository.AddAsync(sale);

            sale.Quantity = 5;
            await _repository.UpdateAsync(sale);

            var updated = await _repository.GetByIdAsync(sale.SaleId);

            Assert.That(updated, Is.Not.Null);
            Assert.That(updated!.Quantity, Is.EqualTo(5));
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveSale()
        {
            var sale = new Sale { ProductId = _defaultProduct.ProductId, Quantity = 1, SalePrice = 50m, PaymentMethod = "Готівка" };
            await _repository.AddAsync(sale);

            await _repository.DeleteAsync(sale.SaleId);
            var result = await _repository.GetByIdAsync(sale.SaleId);

            Assert.That(result, Is.Null);
        }
    }
}