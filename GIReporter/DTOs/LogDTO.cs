namespace GIReporter.DTOs
{
    public class LogDTO
    {
        public string? Time { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        public string? IpAddress { get; set; } = "none";
        public string? Exception { get; set; } = "none";
        public string? RequestMethod { get; set; } = "none";
        public string? RequestPath { get; set; } = "none";
        public string? RequestBodyText { get; set; } = "none";
        public string? StatusCode {get; set;} = "none";
        public string? Duration { get; set; } = "ms";
    }
}
