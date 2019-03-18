using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CategoryService : BaseService<Category>, ICategoryService
    {
        public CategoryService(TodoContext context) : base(context)
        {
        }

        public override async Task<Category> Edit(Guid id, Category category)
        {
            var entity = await GetById(id);
            if (entity == null)
            {
                return null;
            }

            entity.DisplayName = category.DisplayName;

            entity.Modified = DateTime.Now;
            //TODO item.ModifiedById 

            _dbContext.Entry(entity).Property("RowVersion").OriginalValue = category.RowVersion;
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<Category> GetByDisplayName(string name)
        {
            return await _dbContext.Categories.FirstOrDefaultAsync(c => c.DisplayName == name);
        }
    }
}
