using Infrastructure.Data;

namespace Todo_App.Tests
{
    public class DummyDataDBInitializer
    {
        public DummyDataDBInitializer()
        {
        }

        public void Seed(TodoContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            dbContext.Categories.AddRange(TestHelper.Categories);
            dbContext.TodoItems.AddRange(TestHelper.TodoItems);

            dbContext.SaveChanges();
        }
    }
}
