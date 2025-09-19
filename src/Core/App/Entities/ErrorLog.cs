using Core.App.DTOs.Common;

namespace Core.App.Entities;

public class ErrorLog : BaseEntity
{
    public string Message { get; set; }
    public string StackTrace { get; set; }
    public string Source { get; set; }
    
    public string Path { get; set; }
    public string HttpMethod { get; set; }
    public string Target { get; set; }       // Controller.Action
    public string RouteData { get; set; }    // route params
    public string QueryString { get; set; }
    public string Headers { get; set; }
    public string Body { get; set; }
    
    public long? UserId { get; set; }
    public string UserName { get; set; }
    public string UserRoles { get; set; }
    
    public string LogLevel { get; set; } = "Error";
}