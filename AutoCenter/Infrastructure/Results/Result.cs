namespace AutoCenter.Web.Infrastructure.Results
{
    public class Result
    {
        public bool IsSuccess { get; init; }
        public string? Error { get; init; }

        public static Result Ok() => new() { IsSuccess = true };
        public static Result Fail(string error) => new() { IsSuccess = false, Error = error };
    }
}
