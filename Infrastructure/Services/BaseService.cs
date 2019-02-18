using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Services
{
    public abstract class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        protected readonly TodoContext _dbContext;

        public BaseService(TodoContext context)
        {
            _dbContext = context;
        }

        public T Create(T item)
        {
            item.Id = Guid.NewGuid();
            item.Created = DateTime.Now;
            //TODO entity.CreatedById 
            item.Modified = DateTime.Now;
            //TODO entity.ModifiedById 

            _dbContext.Set<T>().Add(item);
            _dbContext.SaveChanges();

            return item;
        }

        public T Delete(Guid id)
        {
            var item = GetById(id);
            if (item == null)
            {
                return null;
            }

            item.IsDeleted = true;
            item.Deleted = DateTime.Now;
            //TODO item.DeletedById

            _dbContext.Entry(item).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return item;
        }

        public abstract T Edit(Guid id, T item);

        public IQueryable<T> GetAll()
        {
            return _dbContext.Set<T>().AsQueryable();
        }

        public T GetById(Guid id)
        {
            return _dbContext.Set<T>().Find(id);
        }
    }
}
