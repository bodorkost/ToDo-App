using Core.Entities;
using System;
using System.Collections.Generic;

namespace Infrastructure.Interfaces
{
    public interface IBaseService<T> where T : BaseEntity
    {
        T Create(T item);

        T GetById(Guid id);

        IEnumerable<T> GetAll();

        T Edit(Guid id, T item);

        T Delete(Guid id);
    }
}
