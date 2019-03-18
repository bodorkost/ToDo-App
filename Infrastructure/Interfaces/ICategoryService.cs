using Core.Entities;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface ICategoryService : IBaseService<Category>
    {
        Task<Category> GetByDisplayName(string name);
    }
}
