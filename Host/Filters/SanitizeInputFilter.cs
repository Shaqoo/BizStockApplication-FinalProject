using Ganss.Xss;
using Microsoft.AspNetCore.Mvc.Filters;
using Nest;
using System.Diagnostics;

namespace Host.Filters
{
    public class SanitizeInputFilter : IActionFilter
    {
        private readonly IHtmlSanitizer _sanitizer;
        public SanitizeInputFilter()
        {
            _sanitizer = new HtmlSanitizer();
            _sanitizer.AllowedTags.Clear();
            _sanitizer.AllowedAttributes.Clear();
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var action = context.ActionDescriptor.DisplayName;
            Console.WriteLine($"[HtmlAction] Action executed: {action} At {DateTime.Now}");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument is null)
                    continue;
                var properties = argument.GetType().GetProperties().Where(a => a.PropertyType == typeof(string)
                 && a.CanWrite && a.CanWrite);

                foreach (var property in properties)
                {
                    var value = property.GetValue(argument) as string;

                    if(!string.IsNullOrEmpty(value))
                    {
                        var safeValue = _sanitizer.Sanitize(value);
                        property.SetValue(argument, safeValue);
                    }
                }
            }
        }
    }
}
