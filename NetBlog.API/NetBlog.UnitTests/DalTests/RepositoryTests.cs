using Microsoft.EntityFrameworkCore;
using Moq;
using NetBlog.DAL.Data;
using NetBlog.DAL.Models;
using NetBlog.DAL.Repositories;
using NetBlog.DAL.Specifications.Interfaces;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace NetBlog.UnitTests.DalTests
{
    public class RepositoryTests
    {
        private readonly DbContextOptions<NetBlogDbContext> _options;
        private readonly NetBlogDbContext _context;
        private readonly Repository<TestEntity> _repository;

        public RepositoryTests()
        {
            _options = new DbContextOptionsBuilder<NetBlogDbContext>()
                .UseInMemoryDatabase(databaseName: "NetBlogTestDb")
                .Options;
            _context = new TestDbContext(_options);
            _repository = new Repository<TestEntity>(_context);
        }

        [Fact]
        public async Task Add_WhenCalled_MustAdd()
        {
            _context.Database.EnsureDeleted();
            var repository = new Repository<TestEntity>(_context);
            var testObject = new TestEntity();

            var res = await repository.Add(testObject);

            Assert.NotNull(res);
            Assert.NotEqual(res.Id, Guid.Empty);
            Assert.False(res.IsDeleted);
        }
        [Fact]
        public async Task Delete_WhenExisitngGuid_ShouldMarkAsDeleted()
        {
            _context.Database.EnsureDeleted();
            var testObject = new TestEntity { IsDeleted = false };
            await _context.Set<TestEntity>().AddAsync(testObject);
            await _context.SaveChangesAsync();

            var repository = new Repository<TestEntity>(_context);

            var res = await repository.Delete(testObject.Id);

            Assert.NotNull(res);
            Assert.True(res.IsDeleted);
        }
        [Fact]
        public async Task Delete_WhenNonExisitngGuid_ShouldReturnNull()
        {
            _context.Database.EnsureDeleted();
            var repository = new Repository<TestEntity>(_context);

            var res = await repository.Delete(Guid.NewGuid());

            Assert.Null(res);
        }
        [Fact]
        public async Task Get_WhenCalledWithExistingId_ShouldReturnEntity()
        {
            _context.Database.EnsureDeleted();
            var testObject = new TestEntity { IsDeleted = false };
            await _context.Set<TestEntity>().AddAsync(testObject);
            await _context.SaveChangesAsync();

            var res = await _repository.Get(testObject.Id);

            Assert.NotNull(res);
            Assert.Equal(testObject.Id, res.Id);
            Assert.False(res.IsDeleted);
        }

        [Fact]
        public async Task Get_WhenCalledWithNonExistingId_ShouldReturnNull()
        {
            _context.Database.EnsureDeleted();

            var res = await _repository.Get(Guid.NewGuid());

            Assert.Null(res);
        }

        [Fact]
        public async Task Get_WhenCalledWithDeletedEntity_ShouldReturnNull()
        {
            _context.Database.EnsureDeleted();
            var testObject = new TestEntity {IsDeleted = true };
            await _context.Set<TestEntity>().AddAsync(testObject);
            await _context.SaveChangesAsync();

            var res = await _repository.Get(testObject.Id);

            Assert.Null(res);
        }

        [Fact]
        public async Task GetWithSpec_WhenCalledWithExistingIdAndSpecification_ShouldReturnEntity()
        {
            _context.Database.EnsureDeleted();

            var testObject = new TestEntity { Id = Guid.NewGuid(), IsDeleted = false};
            await _context.Set<TestEntity>().AddAsync(testObject);
            await _context.SaveChangesAsync();

            var specificationMock = new Mock<ISpecification<TestEntity>>();
            specificationMock.Setup(spec => spec.Criteria).Returns(e => e.Id.ToString().Contains(testObject.Id.ToString()[0]));
            specificationMock.Setup(spec => spec.Includes).Returns(new List<Expression<Func<TestEntity, object>>>());
            specificationMock.Setup(spec => spec.OrderBy).Returns((Expression<Func<TestEntity, object>>)null);
            specificationMock.Setup(spec => spec.OrderByAscending).Returns((bool?)null);

            var res = await _repository.Get(testObject.Id, specificationMock.Object);

            Assert.NotNull(res);
            Assert.Equal(testObject.Id, res.Id);
            Assert.False(res.IsDeleted);
            specificationMock.Verify(m => m.Includes, Times.Once);
            specificationMock.Verify(m => m.Criteria, Times.Exactly(2));
            specificationMock.Verify(m => m.OrderBy, Times.Once);
            specificationMock.Verify(m => m.OrderByAscending, Times.Never);
        }

        [Fact]
        public async Task GetAll_WhenCalledWithDefaults_ShouldReturnFirstPage()
        {
            _context.Database.EnsureDeleted();
            for (int i = 0; i < 10; i++)
            {
                await _context.Set<TestEntity>().AddAsync(new TestEntity { IsDeleted = false });
            }
            await _context.SaveChangesAsync();

            var res = await _repository.GetAll();

            Assert.NotNull(res);
            Assert.Equal(5, res.Count);
        }

        [Fact]
        public async Task GetAll_WhenCalledWithSpecificPageNumberAndPageSize_ShouldReturnCorrectPage()
        {
            _context.Database.EnsureDeleted();
            for (int i = 0; i < 10; i++)
            {
                await _context.Set<TestEntity>().AddAsync(new TestEntity { IsDeleted = false });
            }
            await _context.SaveChangesAsync();

            var res = await _repository.GetAll(2, 3);

            Assert.NotNull(res);
            Assert.Equal(3, res.Count);
        }

        [Fact]
        public async Task GetAll_WhenCalledWithEmptyDatabase_ShouldReturnEmptyList()
        {
            _context.Database.EnsureDeleted();
            var res = await _repository.GetAll();

            Assert.NotNull(res);
            Assert.Empty(res);
        }

        [Fact]
        public async Task GetAll_WhenEntitiesAreMarkedAsDeleted_ShouldNotReturnDeletedEntities()
        {
            _context.Database.EnsureDeleted();
            for (int i = 0; i < 5; i++)
            {
                await _context.Set<TestEntity>().AddAsync(new TestEntity { IsDeleted = true });
            }
            await _context.SaveChangesAsync();

            var res = await _repository.GetAll();

            Assert.NotNull(res);
            Assert.Empty(res);
        }

        [Fact]
        public async Task GetAll_WhenMixOfDeletedAndNonDeletedEntities_ShouldReturnOnlyNonDeleted()
        {
            _context.Database.EnsureDeleted();
            for (int i = 0; i < 3; i++)
            {
                await _context.Set<TestEntity>().AddAsync(new TestEntity { IsDeleted = false });
            }
            for (int i = 0; i < 2; i++)
            {
                await _context.Set<TestEntity>().AddAsync(new TestEntity { IsDeleted = true });
            }
            await _context.SaveChangesAsync();

            var res = await _repository.GetAll();

            Assert.NotNull(res);
            Assert.Equal(3, res.Count);
        }
        [Fact]
        public async Task GetAll_WhenInvalidPagination_ShouldReturnEmptyList()
        {
            var res = await _repository.GetAll(-1,-5);

            Assert.Empty(res);
        }
        /*[Fact]
        public async Task GetAllWithSpec_WhenCalledWithValidPagination_ShouldReturnList()
        {
            _context.Database.EnsureDeleted();

            for (int i = 0; i < 5; i++)
            {
                await _context.Set<TestEntity>().AddAsync(new TestEntity { IsDeleted = true });
            }
            await _context.SaveChangesAsync();

            var specificationMock = new Mock<ISpecification<TestEntity>>();
            specificationMock.Setup(spec => spec.Criteria).Returns(e => true);
            specificationMock.Setup(spec => spec.Includes).Returns(new List<Expression<Func<TestEntity, object>>>());
            specificationMock.Setup(spec => spec.OrderBy).Returns((Expression<Func<TestEntity, object>>)null);
            specificationMock.Setup(spec => spec.OrderByAscending).Returns((bool?)null);

            var res = await _repository.GetAll(specificationMock.Object,1,5);

            Assert.NotNull(res);
            Assert.Equal(res.Count(), 5);
            specificationMock.Verify(m => m.Includes, Times.Once);
            specificationMock.Verify(m => m.Criteria, Times.Exactly(2));
            specificationMock.Verify(m => m.OrderBy, Times.Once);
            specificationMock.Verify(m => m.OrderByAscending, Times.Never);
        }*/
        [Fact]
        public async Task GetAllWithSpec_WhenCalledWithInalidPagination_ShouldReturnEmptyList()
        {

            var specificationMock = new Mock<ISpecification<TestEntity>>();
            specificationMock.Setup(spec => spec.Criteria).Returns(e => true);
            specificationMock.Setup(spec => spec.Includes).Returns(new List<Expression<Func<TestEntity, object>>>());
            specificationMock.Setup(spec => spec.OrderBy).Returns((Expression<Func<TestEntity, object>>)null);
            specificationMock.Setup(spec => spec.OrderByAscending).Returns((bool?)null);

            var res = await _repository.GetAll(specificationMock.Object, -1, -15);

            Assert.Empty(res);
        }
        [Fact]
        public async Task Update_WhenEntityExists_ShouldUpdateEntity()
        {
            _context.Database.EnsureDeleted();
            var testObject = new TestEntity { IsDeleted = false };
            await _context.Set<TestEntity>().AddAsync(testObject);
            await _context.SaveChangesAsync();
            _context.Entry(testObject).State = EntityState.Unchanged; // simulate it being tracked

            testObject.IsDeleted = true;

            var res = await _repository.Update(testObject);

            Assert.NotNull(res);
            Assert.True(res.IsDeleted);
            var updatedEntity = await _context.Set<TestEntity>().FindAsync(testObject.Id);
            Assert.True(updatedEntity.IsDeleted);
        }

        [Fact]
        public async Task Update_WhenEntityDoesNotExist_ShouldReturnNull()
        {
            _context.Database.EnsureDeleted();
            var testObject = new TestEntity { IsDeleted = false };

            var res = await _repository.Update(testObject);

            Assert.Null(res);
        }

        [Fact]
        public async Task GetAll_WithMatchingSpecification_ShouldReturnEntity()
        {
            _context.Database.EnsureDeleted();
            var testObject = new TestEntity { Id = Guid.NewGuid(), IsDeleted = false };
            await _context.Set<TestEntity>().AddAsync(testObject);
            await _context.SaveChangesAsync();

            var specificationMock = new Mock<ISpecification<TestEntity>>();
            specificationMock.Setup(s => s.Criteria).Returns(e => e.Id == testObject.Id);
            specificationMock.Setup(s => s.Includes).Returns(new List<Expression<Func<TestEntity, object>>>());
            specificationMock.Setup(s => s.OrderBy).Returns(null as Expression<Func<TestEntity, object>>);
            specificationMock.Setup(s => s.OrderByAscending).Returns((bool?)null);

            var result = await _repository.GetAll(specificationMock.Object);

            Assert.NotEmpty(result);
            Assert.Equal(testObject.Id, result[0].Id);
        }

        [Fact]
        public async Task GetAll_WithNonMatchingSpecification_ShouldReturnNull()
        {
            _context.Database.EnsureDeleted();
            var testObject = new TestEntity { Id = Guid.NewGuid(), IsDeleted = false };
            await _context.Set<TestEntity>().AddAsync(testObject);
            await _context.SaveChangesAsync();

            var specificationMock = new Mock<ISpecification<TestEntity>>();
            specificationMock.Setup(s => s.Criteria).Returns(e => e.Id != testObject.Id);
            specificationMock.Setup(s => s.Includes).Returns(new List<Expression<Func<TestEntity, object>>>());
            specificationMock.Setup(s => s.OrderBy).Returns(null as Expression<Func<TestEntity, object>>);
            specificationMock.Setup(s => s.OrderByAscending).Returns((bool?)null);

            var result = await _repository.GetAll(specificationMock.Object);

            Assert.Empty(result);
        }


        public class TestEntity : IEntity
        {
            public Guid Id { get; set; }
            public bool IsDeleted { get; set; }
        }
        public class TestDbContext : NetBlogDbContext
        {
            public TestDbContext(DbContextOptions<NetBlogDbContext> options) : base(options)
            {
            }
            public DbSet<TestEntity> TestEntities { get; }
        }
    }
}
