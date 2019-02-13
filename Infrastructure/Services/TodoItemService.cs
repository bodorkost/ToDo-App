using Infrastructure.Interfaces;
using Core.Models;
using Infrastructure.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class TodoItemService : ITodoItemService
    {
        TodoContext _dbContext;

        public TodoItemService(TodoContext context)
        {
            _dbContext = context;
        }

        public TodoItem Create(TodoItem entity)
        {
            entity.Id = Guid.NewGuid();
            entity.Created = DateTime.Now;
            entity.Creator = "System";
            entity.Modified = DateTime.Now;
            entity.Modifier = "System";

            _dbContext.TodoItems.Add(entity);
            _dbContext.SaveChanges();

            return entity;
        }

        public IEnumerable<TodoItem> GetAll()
        {
            return _dbContext.TodoItems.AsEnumerable();
        }

        public TodoItem GetById(Guid id)
        {
            return _dbContext.TodoItems.Find(id);
        }

        public TodoItem Edit(Guid id, TodoItem entity)
        {
            var item = GetById(id);
            if(item == null)
            {
                return null;
            }

            item.Name = entity.Name;
            item.Description = entity.Description;
            item.Priority = entity.Priority;
            item.Responsible = entity.Responsible;
            item.Deadline = entity.Deadline;
            item.Status = entity.Status;
            item.Category = entity.Category;
            item.ParentId = entity.ParentId;

            item.Modified = DateTime.Now;
            item.Modifier = "System";

            _dbContext.Entry(item).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return item;
        }

        public TodoItem Delete(Guid id)
        {
            var item = GetById(id);
            if (item == null)
            {
                return null;
            }

            item.IsDeleted = true;

            _dbContext.Entry(item).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return item;
        }
    }
}
