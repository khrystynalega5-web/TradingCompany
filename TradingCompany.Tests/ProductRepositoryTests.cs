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
    public class ProductRepositoryTests
    {
        private TradingCompanyDbContext _context = null!;
        private ProductRepository _repository = null!;

        private Category _defaultCategory;
        private Supplier _defaultSupplier;

        [SetUp]
        public async Task SetUp()
        {
            var options = new DbContextOptionsBuilder<TradingCompanyDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TradingCompanyDbContext(options);
            _repository = new ProductRepository(_context);

            _defaultCategory = new Category { CategoryName = "Тестова категорія" };
            _defaultSupplier = new Supplier { CompanyName = "Тестовий постачальник", City = "Львів", ContactPhone = "+380000000000" };

            _context.Categories.Add(_defaultCategory);
            _context.Suppliers.Add(_defaultSupplier);
            await _context.SaveChangesAsync();
        }
        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task AddAsync_ShouldAddProduct()
        {
            var product = new Product
            {
                ProductName = "Ноутбук",
                SKU = "LAPTOP-001",
                UnitPrice = 25000m,
                StockQuantity = 5,
                CategoryId = _defaultCategory.CategoryId,
                SupplierId = _defaultSupplier.SupplierId
            };

            await _repository.AddAsync(product);
            var result = await _repository.GetByIdAsync(product.ProductId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.ProductName, Is.EqualTo("Ноутбук"));
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            await _repository.AddAsync(new Product
            {
                ProductName = "P1",
                SKU = "SKU-P1",
                UnitPrice = 10m,
                StockQuantity = 5,
                CategoryId = _defaultCategory.CategoryId,
                SupplierId = _defaultSupplier.SupplierId
            });

            await _repository.AddAsync(new Product
            {
                ProductName = "P2",
                SKU = "SKU-P2",
                UnitPrice = 20m,
                StockQuantity = 5,
                CategoryId = _defaultCategory.CategoryId,
                SupplierId = _defaultSupplier.SupplierId
            });

            var list = await _repository.GetAllAsync();

            Assert.That(list.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateProduct()
        {
            var product = new Product
            {
                ProductName = "Телефон",
                SKU = "PHONE-001",
                UnitPrice = 10000m,
                StockQuantity = 5,
                CategoryId = _defaultCategory.CategoryId,
                SupplierId = _defaultSupplier.SupplierId
            };

            await _repository.AddAsync(product);

            product.UnitPrice = 12000m;
            await _repository.UpdateAsync(product);

            var updated = await _repository.GetByIdAsync(product.ProductId);

            Assert.That(updated, Is.Not.Null);
            Assert.That(updated!.UnitPrice, Is.EqualTo(12000m));
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveProduct()
        {
            var product = new Product
            {
                ProductName = "Delete Me",
                SKU = "DELETE-SKU-001",
                UnitPrice = 50m,
                StockQuantity = 5,
                CategoryId = _defaultCategory.CategoryId,
                SupplierId = _defaultSupplier.SupplierId
            };

            await _repository.AddAsync(product);

            await _repository.DeleteAsync(product.ProductId);
            var result = await _repository.GetByIdAsync(product.ProductId);

            Assert.That(result, Is.Null);
        }
    }
}