namespace API.ErrorHandling
{
    public class ServerException : ServerResponse
    {
        public ServerException(int statusCode, string message = null, string details = null) : base(statusCode, message)
        {
            Details = details;
        }
        public string Details { get; set; }
    }
}