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

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<TradingCompanyDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TradingCompanyDbContext(options);
            _repository = new SaleRepository(_context);
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
            
            var product = new Product { ProductName = "Тестовий товар", UnitPrice = 100m };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var sale = new Sale
            {
                ProductId = product.ProductId,
                Quantity = 2,
                SalePrice = 100m,
                PaymentMethod = "Готівка"
            };

            
            await _repository.AddAsync(sale);
            var result = await _repository.GetByIdAsync(sale.SaleId);

            
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Quantity, Is.EqualTo(2));
            Assert.That(result.ProductId, Is.EqualTo(product.ProductId));
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllSales()
        {
            var product = new Product { ProductName = "Товар", UnitPrice = 50m };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            await _repository.AddAsync(new Sale { ProductId = product.ProductId, Quantity = 1, SalePrice = 50m, PaymentMethod = "Картка" });
            await _repository.AddAsync(new Sale { ProductId = product.ProductId, Quantity = 3, SalePrice = 50m, PaymentMethod = "Готівка" });

            var list = await _repository.GetAllAsync();

            Assert.That(list.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateSale()
        {
            var product = new Product { ProductName = "Товар", UnitPrice = 50m };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var sale = new Sale { ProductId = product.ProductId, Quantity = 1, SalePrice = 50m, PaymentMethod = "Готівка" };
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
            var product = new Product { ProductName = "Товар", UnitPrice = 50m };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var sale = new Sale { ProductId = product.ProductId, Quantity = 1, SalePrice = 50m, PaymentMethod = "Готівка" };
            await _repository.AddAsync(sale);

            await _repository.DeleteAsync(sale.SaleId);
            var result = await _repository.GetByIdAsync(sale.SaleId);

            Assert.That(result, Is.Null);
        }
    }
}