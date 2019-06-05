namespace URLShortenerAPI.Data
{
    public class UrlConfig
    {
        public string UrlPrefix { get; set; }
        public int DaysUntilExpire { get; set; }
    }
}
