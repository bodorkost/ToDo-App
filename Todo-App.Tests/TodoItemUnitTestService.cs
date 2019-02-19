using Core.Entities;
using Core.Types;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
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

        #endregion

        #region GetById

        #endregion

        #region GetMyTodosFromSql

        #endregion
    }
}
