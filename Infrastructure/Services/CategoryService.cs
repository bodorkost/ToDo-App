using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Services
{
    public class CategoryService : BaseService<Category>
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

            entity.DisplayName = entity.DisplayName;

            entity.Modified = DateTime.Now;
            //TODO item.ModifiedById 

            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return entity;
        }
    }
}
