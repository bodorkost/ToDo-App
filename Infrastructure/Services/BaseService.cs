using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Infrastructure.Services
{
    public abstract class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        protected readonly TodoContext _dbContext;

        public BaseService(TodoContext context)
        {
            _dbContext = context;
        }

        public async Task<T> Create(T item)
        {
            item.Id = Guid.NewGuid();
            item.Created = DateTime.Now;
            //TODO entity.CreatedById 
            item.Modified = DateTime.Now;
            //TODO entity.ModifiedById 

            _dbContext.Set<T>().Add(item);
            await _dbContext.SaveChangesAsync();

            return item;
        }

        public async Task<T> Delete(Guid id)
        {
            var item = await GetById(id);
            if (item == null)
            {
                return null;
            }

            item.IsDeleted = true;
            item.Deleted = DateTime.Now;
            //TODO item.DeletedById

            _dbContext.Entry(item).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return item;
        }

        public abstract Task<T> Edit(Guid id, T item);

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(Guid id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
    }
}
