using System.Text;

namespace URLShortenerWeb.Helpers
{
    public static class ShortURLCodeGenerator
    {
        const string characterSet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"; //available characters for the random short URL code to use.

        private static readonly Random random = new ();
        public static string GenerateShortURLCode()
        {
            StringBuilder shortCode = new (8);
            for (int i = 0; i < 8; i++)
            {
                shortCode.Append(characterSet[random.Next(characterSet.Length)]); //this is choosing a random character from the characterSet and appending it to the stringbuilder, 8 times.
            }
            return shortCode.ToString();
        }
       
    }
}
