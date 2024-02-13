namespace URLShortenerWeb.Data
{
    public interface IURLRepository
    {
        string GetOriginalURL(string shortURLCode);
        string AddUrl(Url url);
        bool DupCheck(string shortURLCode);
        void SaveChanges();
        void IncrementCount(string shortURLCode);
        Url GetUrlRecord(string shortURLCode);
    }
}
