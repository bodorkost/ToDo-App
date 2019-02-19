using Core.Entities;
using Core.Types;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace Todo_App.Tests
{
    public class TodoItemUnitTestService
    {
        private readonly ITodoItemService _todoItemService;
        public static DbContextOptions<TodoContext> _dbContextOptions { get; }
        public static string connectionString = "";

        static TodoItemUnitTestService()
        {
            _dbContextOptions = new DbContextOptionsBuilder<TodoContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        public TodoItemUnitTestService()
        {
            var context = new TodoContext(_dbContextOptions);
            DummyDataDBInitializer db = new DummyDataDBInitializer();
            db.Seed(context);

            _todoItemService = new TodoItemService(context);
        }

        #region Create

        [Fact]
        public void Create_ValidData_TodoItem()
        {
            //Arrange  
            var item = new TodoItem() { Name = "Todo 5", Priority = Priority.CRITICAL };

            //Act  
            var data = _todoItemService.Create(item);

            //Assert  
            Assert.NotEqual(Guid.Empty, data.Id);
        }

        [Fact]
        public void Create_InvalidData_DbUpdateException()
        {
            //Arrange  
            var item = new TodoItem() { Priority = Priority.CRITICAL, Created = DateTime.Now, Modified = DateTime.Now };

            //Act & Assert  
            Assert.Throws<DbUpdateException>(() => _todoItemService.Create(item));
        }

        #endregion

        #region Delete

        #endregion

        #region Edit

        #endregion

        #region GetAll

        [Fact]
        public void GetAll_Return_TodoItems()
        {
            //Act  
            var data = _todoItemService.GetAll();

            //Assert  
            Assert.IsAssignableFrom<IQueryable<TodoItem>>(data);
        }

        [Fact]
        public void Read_CountTodoItems_Match()
        {
            //Act  
            var data = _todoItemService.GetAll();

            //Assert
            Assert.Equal(4, data.Count());
        }

        #endregion

        #region GetById

        [Fact]
        public void GetById_ValidId_TodoItem()
        {
            //Arrange
            var item = TestHelper.TodoItems.ElementAt(0);
            var id = item.Id;

            //Act  
            var data = _todoItemService.GetById(id);

            //Assert  
            Assert.Equal(item, data);
        }

        [Fact]
        public void GetById_InvalidId_Null()
        {
            //Arrange  
            var id = Guid.Empty;

            //Act  
            var data = _todoItemService.GetById(id);

            //Assert  
            Assert.Null(data);
        }

        #endregion

        #region GetMyTodosFromSql

        #endregion
    }
}
