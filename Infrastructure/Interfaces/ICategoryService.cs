using Core.Entities;
using System;
using System.Collections.Generic;

namespace Infrastructure.Interfaces
{
    public interface ICategoryService
    {
        Category Create(Category category);

        Category GetById(Guid id);

        IEnumerable<Category> GetAll();

        Category Edit(Guid id, Category category);

        Category Delete(Guid id);
    }
}
