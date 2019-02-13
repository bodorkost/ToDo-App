using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ToDo_App.Extensions
{
    public static class ModelStateDictionaryExtensions
    {
        public static IEnumerable<object> GetErrors(this ModelStateDictionary modelState)
        {
            return modelState
                    .Where(p => p.Value.ValidationState == ModelValidationState.Invalid)
                    .Select(p => new { key = p.Key, propErrors = p.Value.Errors.Select(e => e.ErrorMessage) })
                    .AsEnumerable();
        }
    }
}
