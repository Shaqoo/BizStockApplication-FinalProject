namespace Domain.Exceptions
{
   
    public class BizStockException : Exception
    {
        public int StatusCode { get; }

        public BizStockException(string message, int statusCode = 422)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public BizStockException(string message, Exception innerException, int statusCode = 422)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}
