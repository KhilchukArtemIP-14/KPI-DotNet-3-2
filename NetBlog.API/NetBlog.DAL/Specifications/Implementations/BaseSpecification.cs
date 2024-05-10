using NetBlog.DAL.Models;
using NetBlog.DAL.Specifications.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.DAL.Specifications.Implementations
{
    public class BaseSpecification<T> : ISpecification<T> where T : class, IEntity
    {
        public BaseSpecification()
        {
            _includes = new List<Expression<Func<T, object>>>();
        }
        public BaseSpecification(Expression<Func<T, bool>> criteria) {
            _criteria = criteria;
            _includes = new List<Expression<Func<T, object>>>();
        }
        private Expression<Func<T, bool>> _criteria;
        private List<Expression<Func<T, object>>> _includes;
        public Expression<Func<T, bool>> Criteria => _criteria;

        public List<Expression<Func<T, object>>> Includes => _includes;

        public void AddInclude(Expression<Func<T, object>> include) { Includes.Add(include); }
    }
}
