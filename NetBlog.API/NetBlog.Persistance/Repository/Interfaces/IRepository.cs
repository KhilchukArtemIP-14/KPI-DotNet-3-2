using NetBlog.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Persistance.Repository.Interfaces
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<List<T>> GetAll(int pageNumber = 1, int pageSize = 5);
        Task<List<T>> GetAll(ISpecification<T> specification, int pageNumber = 1, int pageSize = 5);
        Task<T> Get(Guid id);
        Task<T> Get(Guid id, ISpecification<T> specification);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(Guid id);
    }
}
