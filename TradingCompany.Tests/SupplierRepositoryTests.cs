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

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<TradingCompanyDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TradingCompanyDbContext(options);
            _repository = new SupplierRepository(_context);
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
            // Arrange
            var supplier = new Supplier
            {
                CompanyName = "TechCorp"
            };

            // Act
            await _repository.AddAsync(supplier);
            var result = await _repository.GetByIdAsync(supplier.SupplierId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.CompanyName, Is.EqualTo("TechCorp"));
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllSuppliers()
        {
            // Arrange
            await _repository.AddAsync(new Supplier
            {
                CompanyName = "Supplier A"
            });

            await _repository.AddAsync(new Supplier
            {
                CompanyName = "Supplier B"
            });

            // Act
            var list = await _repository.GetAllAsync();

            // Assert
            Assert.That(list.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateSupplier()
        {
            // Arrange
            var supplier = new Supplier
            {
                CompanyName = "Old Supplier"
            };

            await _repository.AddAsync(supplier);

            // Act
            supplier.CompanyName = "New Supplier";
            await _repository.UpdateAsync(supplier);

            var updated = await _repository.GetByIdAsync(supplier.SupplierId);

            // Assert
            Assert.That(updated, Is.Not.Null);
            Assert.That(updated!.CompanyName, Is.EqualTo("New Supplier"));
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveSupplier()
        {
            // Arrange
            var supplier = new Supplier
            {
                CompanyName = "To Delete"
            };

            await _repository.AddAsync(supplier);

            // Act
            await _repository.DeleteAsync(supplier.SupplierId);

            var result = await _repository.GetByIdAsync(supplier.SupplierId);

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}