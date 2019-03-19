using Infrastructure.Interfaces;
using Core.Entities;
using Infrastructure.Data;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Snickler.EFCore;
using SolrNet;

namespace Infrastructure.Services
{
    public class TodoItemService : BaseService<TodoItem>, ITodoItemService
    {
        private readonly ISolrOperations<TodoItem> _solr;

        public TodoItemService(TodoContext context, ISolrOperations<TodoItem> solr) : base(context)
        {
            _solr = solr;
            SolrRefresh();
        }

        private void InitSolr()
        {
            _solr.Delete(SolrQuery.All);
            _solr.AddRange(_dbContext.TodoItems.AsEnumerable());
            _solr.Commit();
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
            IEnumerable<TodoItem> result = null;

            _dbContext.LoadStoredProc("dbo.GetTodosByResponsible")
                      .WithSqlParam("Responsible", responsible)
                      .ExecuteStoredProc((handler) =>
                      {
                          result = handler.ReadToList<TodoItem>();
                      });

            return result;

            #region Other solutions

            //if (!string.IsNullOrEmpty(connectionString))
            //{
            //    DataTable dataTable = new DataTable();
            //    using (SqlConnection sqlConn = new SqlConnection(connectionString))
            //    {
            //        string sql = "GetTodosByResponsible";
            //        using (SqlCommand sqlCmd = new SqlCommand(sql, sqlConn))
            //        {
            //            sqlCmd.CommandType = CommandType.StoredProcedure;
            //            sqlCmd.Parameters.AddWithValue("@Responsible", responsible);
            //            sqlConn.Open();
            //            using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd))
            //            {
            //                sqlAdapter.Fill(dataTable);
            //            }
            //        }
            //    }

            //    foreach (DataRow row in dataTable.Rows)
            //    {
            //        yield return row.ToObject<TodoItem>();
            //    }
            //}

            //return _dbContext.TodoItems
            //                 .FromSql($"GetTodosByResponsible {responsible}")
            //                 .AsEnumerable();

            #endregion
        }

        public IEnumerable<TodoItem> SolrSearch(string searchText)
        {
            var condition = string.IsNullOrEmpty(searchText) ? "*:*" : $"Name:{searchText} OR Description:{searchText} OR Responsible:{searchText} OR Status:{searchText} OR Priority:{searchText}";
            return _solr.Query(new SolrQuery(condition)).ToList();
        }

        public void SolrRefresh()
        {
            _solr.Delete(SolrQuery.All);
            _solr.AddRange(_dbContext.TodoItems.AsEnumerable());
            _solr.Commit();
        }
    }
}
