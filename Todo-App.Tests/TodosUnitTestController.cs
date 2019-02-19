using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using ToDo_App.Controllers;
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

        [Fact]
        public void Read_Return_OkResult()
        {
            //Arrange  
            var controller = new TodosController(_todoItemService, null, null);

            //Act  
            var data = controller.Read();

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public void Read_CountTodos_MatchResult()
        {
            //Arrange  
            var controller = new TodosController(_todoItemService, null, null);

            //Act  
            var data = controller.Read();

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = (OkObjectResult)data;
            var queryableItems = (IQueryable<TodoItem>)okResult.Value;
            var itemsCount = queryableItems.AsEnumerable().Count();

            //Assert
            Assert.Equal(4, itemsCount);
        }

        #endregion

        #region Update

        #endregion

        #region Delete

        #endregion
    }
}
