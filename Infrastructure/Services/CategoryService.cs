using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Infrastructure.Services
{
    public class CategoryService : BaseService<Category>, ICategoryService
    {
        public CategoryService(TodoContext context) : base(context)
        {
        }

        public override Category Edit(Guid id, Category category)
        {
            var entity = GetById(id);
            if (entity == null)
            {
                return null;
            }

            entity.DisplayName = category.DisplayName;

            entity.Modified = DateTime.Now;
            //TODO item.ModifiedById 

            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return entity;
        }

        public Category GetByDisplayName(string name)
        {
            return _dbContext.Categories.FirstOrDefault(c => c.DisplayName == name);
        }
    }
}
