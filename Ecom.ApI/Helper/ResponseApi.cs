namespace Ecom.ApI.Helper
{
    public class ResponseApi
    {
        public ResponseApi(int statusCode, string message=null)
        {
            StatusCode = statusCode;
            Message = message??GetMessageFromStatusCode(statusCode);
        }
        private string GetMessageFromStatusCode(int statusCode)
        {
            return statusCode switch
            {
                200 => "Success",
                201 => "Created",
                204 => "No Content",
                400 => "Bad Request",
                401 => "Un Authorized",
                403 => "Forbidden",
                404 => "Not Found",
                500 => "Internal Server Error",
                _ => "Unknown"
            };
        }
        public int StatusCode { get; set; }
        public string? Message { get; set; }
    }
}
