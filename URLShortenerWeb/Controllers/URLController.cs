using Microsoft.AspNetCore.Mvc;
using URLShortenerWeb.Data;
using URLShortenerWeb.Services;

namespace URLShortenerWeb.Controllers
{
    public class URLController : Controller
    {
        private readonly IShortURLCodeService _shortURLCodeService; //injecting the ShortURLCodeService into the controller.
        private readonly ILogger<URLController> _logger; //injecting the logger service.

        public URLController(IShortURLCodeService shortURLCodeService, ILogger<URLController> logger)
        {
            _shortURLCodeService = shortURLCodeService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GenerateURL()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GenerateURL(Url url)
        {
            try
            {
                if (string.IsNullOrEmpty(url.OriginalUrl))
                {
                    return RedirectToAction("Error"); //pushing the user to an error page, would improve this by adding toastjs notifications, telling the user what they done wrong.
                }
                url.OriginalUrl = url.OriginalUrl.ToLower().Trim(); // the URL should be lower case and not have spaces. (Best practices)

                if (!url.OriginalUrl.StartsWith("http")) //basic validation to ensure they are at least typing http at the start, i would build a regex method to match a URL.
                {
                    return RedirectToAction("Error");
                }
                string newShortURLCode = _shortURLCodeService.SaveNewShortURLCode(url);
                return RedirectToAction("ShowURL", new { shortUrlCode = newShortURLCode });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error encountered on GenerateURL Post action on URL controller. {ex.Message} {ex.InnerException}");
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        public IActionResult ShowURL(string shortUrlCode)
        {
            try
            {
                Url url = _shortURLCodeService.GetUrlRecord(shortUrlCode);

                if (url == null)
                {
                    return RedirectToAction("Error");
                }
                return View(url);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error encountered on ShowURL action on URL controller. {ex.Message} {ex.InnerException}");
                return RedirectToAction("Error");
            }
        }

       
        [HttpGet("/{ShortURLCode}")] //this route is defined in the program.cs file, I would potentially have it on its own route like /go/[ShortURLCode] as it is not great to have something routing from the base URL.
        public IActionResult RedirectTo(string shortURLCode)
        {
            try
            {
                if (string.IsNullOrEmpty(shortURLCode))
                {
                    return RedirectToAction("GenerateURL"); //Redirect the user back to the generateURL screen if the short URL code can't be found
                }

                string shortUrl = _shortURLCodeService.GetOriginalURL(shortURLCode);
                if (string.IsNullOrEmpty(shortUrl))
                {
                    return RedirectToAction("GenerateURL");
                }

                _shortURLCodeService.IncrementCount(shortURLCode); //incrementing the total hits when the short URL is accessed.

                return Redirect(shortUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error encountered on RedirectTo action on URL controller. {ex.Message} {ex.InnerException}");
                return RedirectToAction("Error");
            }
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
