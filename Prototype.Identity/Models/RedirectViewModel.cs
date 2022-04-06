namespace Prototype.Identity.Models
{
    public class RedirectViewModel
    {
        public string RedirectUrl { get; set; }

        public RedirectViewModel(string url)
        {
            RedirectUrl = url;
        }
    }
}
