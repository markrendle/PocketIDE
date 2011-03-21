namespace PocketCSharp.Services
{
    public class BingSearchResult
    {
        private readonly string _title;
        private readonly string _description;
        private readonly string _msdnUrl;

        public BingSearchResult(string title, string description, string displayUrl)
        {
            _title = title;
            _msdnUrl = displayUrl;
            _description = description.Contains("...") ? description.Substring(0, description.IndexOf("...")).Trim() : description;
        }

        public string MsdnUrl
        {
            get { return _msdnUrl; }
        }

        public string Description
        {
            get { return _description; }
        }

        public string Title
        {
            get { return _title; }
        }
    }
}
