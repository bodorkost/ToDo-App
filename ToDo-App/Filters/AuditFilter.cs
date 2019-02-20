using Core.Settings;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System;
using System.IO;

namespace ToDo_App.Filters
{
    public class AuditFilter : IActionFilter
    {
        private readonly TodoContext _dbContext;
        private readonly IOptions<AuditSettings> _config;

        public AuditFilter(TodoContext dbContext, IOptions<AuditSettings> config)
        {
            _dbContext = dbContext;
            _config = config;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            var entity = new Audit()
            {
                Id = Guid.NewGuid(),
                Url = _config.Value.WithUrl ? request.Path : null,
                Header = _config.Value.WithHeaders ? string.Join(";", request.Headers.Keys) : null,
                Method = _config.Value.WithMethod ? request.Method : null,
                Created = DateTime.Now
            };

            if (_config.Value.WithBody)
            {
                request.EnableRewind();
                request.Body.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(request.Body))
                {
                    entity.Body = reader.ReadToEnd();
                }
            }

            _dbContext.Audits.Add(entity);
            _dbContext.SaveChanges();
        }

        public void OnActionExecuted(ActionExecutedContext context) { }

    }
}
