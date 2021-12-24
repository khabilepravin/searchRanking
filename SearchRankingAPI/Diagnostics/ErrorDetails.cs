namespace SearchRankingAPI.Diagnostics
{
    public record ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
