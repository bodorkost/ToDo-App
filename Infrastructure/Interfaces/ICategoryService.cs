using Core.Entities;

namespace Infrastructure.Interfaces
{
    public interface ICategoryService : IBaseService<Category>
    {
        Category GetByDisplayName(string name);
    }
}
