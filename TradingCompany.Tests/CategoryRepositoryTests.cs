using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using TradingCompany.DAL.Models;
using TradingCompany.DAL.Repositories.Repository;

namespace TradingCompany.Tests
{
    public class CategoryRepositoryTests
    {
        private TradingCompanyDbContext _context = null!;
        private CategoryRepository _repository = null!;
        private Category _defaultCategory = null!;

        [SetUp]
        public async Task SetUp()
        {
            var options = new DbContextOptionsBuilder<TradingCompanyDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TradingCompanyDbContext(options);
            _repository = new CategoryRepository(_context);

            _defaultCategory = new Category
            {
                CategoryName = "Тестова категорія",
                Description = "Опис тестової категорії"
            };

            _context.Categories.Add(_defaultCategory);
            await _context.SaveChangesAsync();
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
            var newCategory = new Category
            {
                CategoryName = "Електроніка",
                Description = "Побутова техніка"
            };

            await _repository.AddAsync(newCategory);
            var result = await _repository.GetByIdAsync(newCategory.CategoryId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.CategoryName, Is.EqualTo("Електроніка"));
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllCategories()
        {
            await _repository.AddAsync(new Category { CategoryName = "Одяг" });

            var list = await _repository.GetAllAsync();

            Assert.That(list.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateExistingCategory()
        {
            _defaultCategory.CategoryName = "Оновлена назва";
            await _repository.UpdateAsync(_defaultCategory);

            var updated = await _repository.GetByIdAsync(_defaultCategory.CategoryId);

            Assert.That(updated, Is.Not.Null);
            Assert.That(updated!.CategoryName, Is.EqualTo("Оновлена назва"));
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveCategoryFromDatabase()
        {
            await _repository.DeleteAsync(_defaultCategory.CategoryId);

            var result = await _repository.GetByIdAsync(_defaultCategory.CategoryId);

            Assert.That(result, Is.Null);
        }
    }
}