using URLShortenerWeb.Data;

namespace URLShortenerWeb.Services
{
    public interface IShortURLCodeService
    {
        string SaveNewShortURLCode(Url url);
        string GetOriginalURL(string shortURLCode);
        void IncrementCount(string shortURLCode);
        Url GetUrlRecord(string shortURLCode);
    }
}
