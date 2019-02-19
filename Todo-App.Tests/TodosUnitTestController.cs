using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace Todo_App.Tests
{
    public class TodosUnitTestController
    {
        private ITodoItemService _todoItemService;
        public static DbContextOptions<TodoContext> _dbContextOptions { get; }
        public static string connectionString = "";

        static TodosUnitTestController()
        {
            _dbContextOptions = new DbContextOptionsBuilder<TodoContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        public TodosUnitTestController()
        {
            var context = new TodoContext(_dbContextOptions);
            DummyDataDBInitializer db = new DummyDataDBInitializer();
            db.Seed(context);

            _todoItemService = new TodoItemService(context);
        }

        #region Create

        #endregion

        #region Read By Id

        #endregion

        #region Read All

        #endregion

        #region Update

        #endregion

        #region Delete

        #endregion
    }
}
