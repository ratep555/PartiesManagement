namespace API.ErrorHandling
{
    public class ServerResponse
    {
        public ServerResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;

            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }       
        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {          
            return statusCode switch
            {
                400 => "Sorry, you have made a bad request",
                401 => "Sorry, not authorized",
                404 => "Sorry, resource not found",
                500 => "Sorry, there was an internal server error",
                _ => null
            };
        }
    }
}