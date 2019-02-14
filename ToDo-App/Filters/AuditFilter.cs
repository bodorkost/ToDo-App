using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IO;

namespace ToDo_App.Filters
{
    public class AuditFilter : IActionFilter
    {
        private readonly TodoContext _dbContext;

        public AuditFilter(TodoContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            var entity = new Audit()
            {
                Id = Guid.NewGuid(),
                Url = request.Path,
                Header = string.Join(";", request.Headers.Keys)
            };

            request.EnableRewind();
            request.Body.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(request.Body))
            {
                entity.Body = reader.ReadToEnd();
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }

    }
}
