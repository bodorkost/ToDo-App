using Core.Settings;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ToDo_App.Filters
{
    public class AuditFilter : IAsyncActionFilter
    {
        private readonly TodoContext _dbContext;
        private readonly IOptions<AuditSettings> _config;

        public AuditFilter(TodoContext dbContext, IOptions<AuditSettings> config)
        {
            _dbContext = dbContext;
            _config = config;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;

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
                    entity.Body = await reader.ReadToEndAsync();
                }
            }

            _dbContext.Audits.Add(entity);
            await _dbContext.SaveChangesAsync();

            await next();
        }
    }
}
