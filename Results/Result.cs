namespace HisoBOT.Results
{
    public class Result
    {
        public bool Success { get; init; }
        public string Message { get; init; }

        public Result(bool success, string msg)
        {
            Success = success;
            Message = msg;
        }
    }
}
