using Core.Entities;
using Core.Types;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using ToDo_App.Controllers;
using Xunit;

namespace Todo_App.Tests
{
    public class TodosUnitTestController
    {
        private readonly ITodoItemService _todoItemService;
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

        [Fact]
        public void Create_ValidData_OkResult()
        {
            //Arrange  
            var controller = new TodosController(_todoItemService, null, null);
            var item = new TodoItem() { Name = "Todo 5", Priority = Priority.CRITICAL, Created = DateTime.Now, Modified = DateTime.Now };

            //Act  
            var data = controller.Create(item);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public void Create_InvalidData_BadRequest()
        {
            //Arrange  
            var controller = new TodosController(_todoItemService, null, null);
            var item = new TodoItem() { Priority = Priority.CRITICAL, Created = DateTime.Now, Modified = DateTime.Now };

            controller.ModelState.AddModelError("Name", "Name is required.");

            //Act              
            var data = controller.Create(item);

            //Assert  
            Assert.IsType<BadRequestObjectResult>(data);
        }

        #endregion

        #region Read By Id

        [Fact]
        public void ReadById_Return_OkResult()
        {
            //Arrange  
            var controller = new TodosController(_todoItemService, null, null);
            var id = TestHelper.TodoItems.ElementAt(0).Id;

            //Act  
            var data = controller.Read(id);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public void ReadById_Return_BadRequestResult()
        {
            //Arrange  
            var controller = new TodosController(_todoItemService, null, null);
            var id = Guid.Empty;

            //Act  
            var data = controller.Read(id);

            //Assert  
            Assert.IsType<BadRequestObjectResult>(data);
        }

        [Fact]
        public void ReadById_Return_MatchResult()
        {
            //Arrange  
            var controller = new TodosController(_todoItemService, null, null);
            var id = TestHelper.TodoItems.ElementAt(0).Id;

            //Act  
            var data = controller.Read(id);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = (OkObjectResult)data;
            var item = (TodoItem)okResult.Value;

            //Assert
            Assert.Equal("Todo 1", item.Name);
            Assert.Equal(Priority.IMPORTANT, item.Priority);
        }

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
