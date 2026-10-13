using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using TradingCompany.DAL.Models;
using TradingCompany.DAL.Repositories.Interface;
using TradingCompany.DAL.Repositories.Repository;

namespace TradingCompany.Tests
{
    [TestFixture]
    public class SupplierRepositoryTests
    {
        private TradingCompanyDbContext _context = null!;
        private ISupplierRepository _repository = null!;
        private Supplier _defaultSupplier = null!;

        [SetUp]
        public async Task SetUp()
        {
            var options = new DbContextOptionsBuilder<TradingCompanyDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TradingCompanyDbContext(options);
            _repository = new SupplierRepository(_context);

            _defaultSupplier = new Supplier
            {
                CompanyName = "Тестовий постачальник",
                City = "Львів",
                ContactPhone = "+380000000000"
            };

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
        public async Task AddAsync_ShouldAddSupplierToDatabase()
        {
            var supplier = new Supplier
            {
                CompanyName = "TechCorp",
                City = "Київ",
                ContactPhone = "+380111111111"
            };

            await _repository.AddAsync(supplier);
            var result = await _repository.GetByIdAsync(supplier.SupplierId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.CompanyName, Is.EqualTo("TechCorp"));
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllSuppliers()
        {
            await _repository.AddAsync(new Supplier
            {
                CompanyName = "Supplier A",
                City = "Львів",
                ContactPhone = "+380222222222"
            });

            var list = await _repository.GetAllAsync();

            Assert.That(list.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateSupplier()
        {
            _defaultSupplier.CompanyName = "New Supplier";
            await _repository.UpdateAsync(_defaultSupplier);

            var updated = await _repository.GetByIdAsync(_defaultSupplier.SupplierId);

            Assert.That(updated, Is.Not.Null);
            Assert.That(updated!.CompanyName, Is.EqualTo("New Supplier"));
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveSupplier()
        {
            await _repository.DeleteAsync(_defaultSupplier.SupplierId);

            var result = await _repository.GetByIdAsync(_defaultSupplier.SupplierId);

            Assert.That(result, Is.Null);
        }
    }
}