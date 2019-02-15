using Infrastructure.Interfaces;
using Core.Entities;
using Infrastructure.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class TodoItemService : ITodoItemService
    {
        private TodoContext _dbContext;

        public TodoItemService(TodoContext context)
        {
            _dbContext = context;
        }

        public TodoItem Create(TodoItem entity)
        {
            entity.Id = Guid.NewGuid();
            entity.Created = DateTime.Now;
            //TODO entity.CreatedById 
            entity.Modified = DateTime.Now;
            //TODO entity.ModifiedById 

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
            //TODO item.ModifiedById 

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
            item.Deleted = DateTime.Now;
            //TODO item.DeletedById

            _dbContext.Entry(item).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return item;
        }
    }
}
