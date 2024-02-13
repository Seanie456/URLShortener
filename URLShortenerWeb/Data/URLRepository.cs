using Microsoft.EntityFrameworkCore;
using URLShortenerWeb.Helpers;

namespace URLShortenerWeb.Data
{
    public class URLRepository : IURLRepository 
    {
        protected readonly SQLDbContext _context;

        public URLRepository(SQLDbContext context)
        {
            _context = context; //injecting context through the constructor.
        }

        public string AddUrl(Url url)
        {
            _context.Urls.Add(url);
            return url.ShortenedUrlcode;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public bool DupCheck(string shortURLCode)
        {
            return _context.Urls.Any(x=>x.ShortenedUrlcode == shortURLCode);  //checking if there are any existing short URL codes matching the one that was generated.
        }

        public string GetOriginalURL(string shortURLCode)
        {
            if (string.IsNullOrEmpty(shortURLCode))
            {
                return string.Empty;
            }

            Url record = _context.Urls.FirstOrDefault(x => x.ShortenedUrlcode == shortURLCode); // == is directly translated to SQL. Needs to be an exact text match on the shortURLCode, it is case sensitive.
            if (record != null)
            {
                return record.OriginalUrl;
            }
            return string.Empty;
        }

        public void IncrementCount(string shortURLCode)
        {
            Url record = _context.Urls.FirstOrDefault(x=>x.ShortenedUrlcode == shortURLCode);
            if (record != null)
            {
                record.TotalHits++;
                record.LastAccessed = DateTime.Now; /*updating last access date time. Intention would be to disable any really old entries or entries with zero hits with the IsDeactivated flag, 
                                                     * I wouldn't delete them out of the table to avoid unintended duplicated short URL code */
                _context.Entry(record).State = EntityState.Modified;
                SaveChanges(); //call the save changes already defined.
            }
        }

        public Url GetUrlRecord(string shortURLCode) 
        {
            if (string.IsNullOrEmpty(shortURLCode))
            {
                return null;
            }

            Url record = _context.Urls.FirstOrDefault(x => x.ShortenedUrlcode == shortURLCode);
            if (record != null)
            {
                return record;
            }
            return null; //the null check is handled on the references consuming the return of this method.
        }
    }
}
