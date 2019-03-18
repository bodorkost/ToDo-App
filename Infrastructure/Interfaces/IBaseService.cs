using Core.Entities;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Infrastructure.Interfaces
{
    public interface IBaseService<T> where T : BaseEntity
    {
        Task<T> Create(T item);

        Task<T> GetById(Guid id);

        Task<IEnumerable<T>> GetAll();

        Task<T> Edit(Guid id, T item);

        Task<T> Delete(Guid id);
    }
}
