﻿using Infrastructure.Interfaces;
using Core.Entities;
using Infrastructure.Data;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Services
{
    public class TodoItemService : BaseService<TodoItem>, ITodoItemService
    {
        public TodoItemService(TodoContext context) : base(context)
        {
        }

        public override TodoItem Edit(Guid id, TodoItem entity)
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

            _dbContext.Entry(item).Property("RowVersion").OriginalValue = entity.RowVersion;
            _dbContext.Entry(item).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return item;
        }

        public IEnumerable<TodoItem> GetMyTodosFromSql(string responsible)
        {
            return _dbContext.TodoItems
                             .FromSql($"GetTodosByResponsible {responsible}")
                             .AsEnumerable();
        }
    }
}
