namespace Core.Errors;

public class AppException : Exception
{
    public int StatusCode { get; set; }
    
    public AppException(int statusCode, string message = null) : 
        base(message ?? GetDefaultMessageForStatusCode(statusCode))
    {
        StatusCode = statusCode;
    }
    
    public AppException(string message = null) : 
        base(message ?? GetDefaultMessageForStatusCode(400))
    {
        StatusCode = 400;
    }

    private static string GetDefaultMessageForStatusCode(int statusCode)
    {
        return statusCode switch
        {
            400 => "Bad Request",
            401 => "Unauthorized",
            403 => "Forbidden", 
            404 => "Not Found",
            409 => "Conflict",
            422 => "Unprocessable Entity",
            500 => "Internal Server Error",
            502 => "Bad Gateway",
            503 => "Service Unavailable",
            _ => "An error occurred"
        };
    }
}