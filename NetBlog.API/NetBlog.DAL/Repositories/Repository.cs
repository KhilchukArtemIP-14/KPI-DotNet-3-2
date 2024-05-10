﻿using Microsoft.EntityFrameworkCore;
using NetBlog.DAL.Data;
using NetBlog.DAL.Models;
using NetBlog.DAL.Specifications.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.DAL.Repositories
{
    internal class Repository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly NetBlogDbContext _context;
        public Repository(NetBlogDbContext context) { 
            _context = context;
        }
        public async Task<T> Add(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> Delete(Guid id)
        {
            var entity = await _context.Set<T>().Where(t => !t.IsDeleted).FirstOrDefaultAsync(t => t.Id == id);
            entity.IsDeleted = true;

            await _context.SaveChangesAsync();
            return entity;
        }

        public Task<T> Get(Guid id, ISpecification<T> specification)
        {
            return ApplySpecifications(_context.Set<T>().AsQueryable(), specification)
                .Where(t=>!t.IsDeleted)
                .FirstOrDefaultAsync(t=>t.Id==id);
        }

        public Task<T> Get(Guid id)
        {
            return _context.Set<T>().Where(t => !t.IsDeleted).FirstOrDefaultAsync(t => t.Id == id);
        }

        public Task<List<T>> GetAll()
        {
            return _context.Set<T>().Where(t => !t.IsDeleted).ToListAsync();
        }

        public Task<List<T>> GetAll(ISpecification<T> specification)
        {
            return ApplySpecifications(_context.Set<T>().AsQueryable(),specification).Where(t => !t.IsDeleted).ToListAsync();
        }

        public async Task<T> Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
        private IQueryable<T> ApplySpecifications(IQueryable<T> inputQuery,ISpecification<T> specification)
        {
            var query = inputQuery;

            if (specification.Criteria != null)
            {
                query= query.Where(specification.Criteria);
            }

            query = specification.Includes.Aggregate(query,(current, include)=> current.Include(include));

            return query;
        }
    }
}
