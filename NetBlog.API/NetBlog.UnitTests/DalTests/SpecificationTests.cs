using NetBlog.DAL.Models;
using NetBlog.DAL.Specifications.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static NetBlog.UnitTests.DalTests.RepositoryTests;

namespace NetBlog.UnitTests.DalTests
{
    public class SpecificationTests
    {
        [Fact]
        public void Constructor_WithoutCriteria_ShouldInitializeEmptyIncludes()
        {
            var spec = new BaseSpecification<TestEntity>();

            Assert.NotNull(spec.Includes);
            Assert.Empty(spec.Includes);
            Assert.Null(spec.Criteria);
            Assert.Null(spec.OrderBy);
            Assert.Null(spec.OrderByAscending);
        }

        [Fact]
        public void Constructor_WithCriteria_ShouldInitializeWithCriteria()
        {
            Expression<Func<TestEntity, bool>> criteria = e => e.IsDeleted == false;

            var spec = new BaseSpecification<TestEntity>(criteria);

            Assert.Equal(criteria, spec.Criteria);
            Assert.NotNull(spec.Includes);
            Assert.Empty(spec.Includes);
            Assert.Null(spec.OrderBy);
            Assert.Null(spec.OrderByAscending);
        }

        [Fact]
        public void AddInclude_ShouldAddIncludeExpression()
        {
            var spec = new BaseSpecification<TestEntity>();
            Expression<Func<TestEntity, object>> include = e => e;

            spec.AddInclude(include);

            Assert.Single(spec.Includes);
            Assert.Contains(include, spec.Includes);
        }

        [Fact]
        public void AddOrderBy_ShouldSetOrderByAndOrderByAscending()
        {
            var spec = new BaseSpecification<TestEntity>();
            Expression<Func<TestEntity, object>> orderBy = e => e.IsDeleted;

            spec.AddOrderBy(orderBy, true);

            Assert.Equal(orderBy, spec.OrderBy);
            Assert.True(spec.OrderByAscending);
        }

        [Fact]
        public void AddOrderBy_ShouldSetOrderByAndOrderByDescending()
        {
            var spec = new BaseSpecification<TestEntity>();
            Expression<Func<TestEntity, object>> orderBy = e => e.IsDeleted;

            spec.AddOrderBy(orderBy, false);

            Assert.Equal(orderBy, spec.OrderBy);
            Assert.False(spec.OrderByAscending);
        }
        [Fact]
        public void PostWithCommentsSpec_WhenInvalidCommentsToFetch_ShouldThrowException()
        {
            var exception = Assert.Throws<Exception>(() => new PostWithCommentsSpecification(-1));
            Assert.Equal("Wrong comments number", exception.Message);
        }

        [Fact]
        public void Constructor_WhenValidCommentsToFetch_ShouldInitializeCorrectly()
        {
            var commentsToFetch = 5;

            var spec = new PostWithCommentsSpecification(commentsToFetch);

            Assert.Single(spec.Includes);
            Assert.NotEmpty(spec.Includes);
        }
    }
}
