using URLShortenerWeb.Data;
using URLShortenerWeb.Helpers;

namespace URLShortenerWeb.Services
{
    public class ShortURLCodeService : IShortURLCodeService
    {
        private readonly IURLRepository _urlRepository;

        public ShortURLCodeService(IURLRepository urlRepository)
        {
            _urlRepository = urlRepository;
        }

        public string SaveNewShortURLCode(Url url)
        {
            string shortCode = GenerateShortCodeAndDbDupCheck();
            url.ShortenedUrlcode = shortCode;
            _urlRepository.AddUrl(url);
            _urlRepository.SaveChanges();
            return url.ShortenedUrlcode;
        }

        private string GenerateShortCodeAndDbDupCheck()
        {
            string shortCode;
            bool dbCodeDup;

            do
            {
                shortCode = ShortURLCodeGenerator.GenerateShortURLCode();
                dbCodeDup = _urlRepository.DupCheck(shortCode); //this checks if the short code generated above is unique, if not, it will execute again.
            }
            while (dbCodeDup);
            return shortCode;
        }

        public string GetOriginalURL(string shortURLCode)
        {
           return _urlRepository.GetOriginalURL(shortURLCode);
        }

        public void IncrementCount(string shortURLCode)
        {
           _urlRepository.IncrementCount(shortURLCode);
        }

        public Url GetUrlRecord(string shortURLCode)
        {
            return _urlRepository.GetUrlRecord(shortURLCode);
        }

    }
}
