using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class ErrorDto
    {
        public string Source { get; set; } = "Unknown";
        public string Message { get; set; } = "An error occurred.";
        public int StatusCode { get; set; }
        public string TraceId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

}
