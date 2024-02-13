using System;
using System.Collections.Generic;

namespace URLShortenerWeb.Data
{
    public partial class Url
    {
        public long Id { get; set; }
        public string OriginalUrl { get; set; } = null!;
        public string ShortenedUrlcode { get; set; } = null!;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? LastAccessed { get; set; }
        public int TotalHits { get; set; } = 0;
        public bool IsDeactivated { get; set; } = false;
    }
}
