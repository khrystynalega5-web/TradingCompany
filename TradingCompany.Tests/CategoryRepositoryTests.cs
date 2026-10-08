using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using TradingCompany.DAL.Models;
using TradingCompany.DAL.Repositories.Repository;

namespace TradingCompany.Tests
{
    [TestFixture]
    public class CategoryRepositoryTests
    {
        private TradingCompanyDbContext _context = null!;
        private CategoryRepository _repository = null!;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<TradingCompanyDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TradingCompanyDbContext(options);
            _repository = new CategoryRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task AddAsync_ShouldAddCategoryToDatabase()
        {
            // Arrange
            var newCategory = new Category
            {
                CategoryName = "Електроніка",
                Description = "Побутова техніка"
            };

            // Act
            await _repository.AddAsync(newCategory);
            var result = await _repository.GetByIdAsync(newCategory.CategoryId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.CategoryName, Is.EqualTo("Електроніка"));
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllCategories()
        {
            // Arrange
            await _repository.AddAsync(new Category
            {
                CategoryName = "Продукти"
            });

            await _repository.AddAsync(new Category
            {
                CategoryName = "Одяг"
            });

            // Act
            var list = await _repository.GetAllAsync();

            // Assert
            Assert.That(list.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateExistingCategory()
        {
            // Arrange
            var category = new Category
            {
                CategoryName = "Стара назва"
            };

            await _repository.AddAsync(category);

            // Act
            category.CategoryName = "Оновлена назва";
            await _repository.UpdateAsync(category);

            var updated = await _repository.GetByIdAsync(category.CategoryId);

            // Assert
            Assert.That(updated, Is.Not.Null);
            Assert.That(updated!.CategoryName, Is.EqualTo("Оновлена назва"));
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveCategoryFromDatabase()
        {
            // Arrange
            var category = new Category
            {
                CategoryName = "Для видалення"
            };

            await _repository.AddAsync(category);

            // Act
            await _repository.DeleteAsync(category.CategoryId);

            var result = await _repository.GetByIdAsync(category.CategoryId);

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}