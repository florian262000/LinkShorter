namespace LinkShorter
{
    public class LinkAddApiPost
    {
        public string targetUrl;
        public string shortLink;

        public string accessToken;

        public override string ToString()
        {
            return targetUrl;
        }
    }
}