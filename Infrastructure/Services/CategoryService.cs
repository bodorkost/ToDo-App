using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private TodoContext _dbContext;

        public CategoryService(TodoContext context)
        {
            _dbContext = context;
        }

        public Category Create(Category category)
        {
            category.Id = Guid.NewGuid();
            category.Created = DateTime.Now;
            //TODO entity.CreatedById 
            category.Modified = DateTime.Now;
            //TODO entity.ModifiedById 

            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();

            return category;
        }

        public Category Delete(Guid id)
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

        public Category Edit(Guid id, Category category)
        {
            var entity = GetById(id);
            if (entity == null)
            {
                return null;
            }

            entity.DisplayName = entity.DisplayName;

            entity.Modified = DateTime.Now;
            //TODO item.ModifiedById 

            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return entity;
        }

        public IEnumerable<Category> GetAll()
        {
            return _dbContext.Categories.AsEnumerable();
        }

        public Category GetById(Guid id)
        {
            return _dbContext.Categories.Find(id);
        }
    }
}
