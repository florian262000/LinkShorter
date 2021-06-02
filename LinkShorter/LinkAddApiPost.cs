namespace LinkShorter
{
    public class LinkAddApiPost
    {
        public string targetUrl { get; set; }
        public string shortPath { get; set; }

        //public string accessToken { get; set; }

        public override string ToString()
        {
            return targetUrl;
        }
    }
}